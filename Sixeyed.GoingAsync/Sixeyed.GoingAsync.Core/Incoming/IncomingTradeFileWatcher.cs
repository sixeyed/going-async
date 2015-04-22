using Sixeyed.GoingAsync.Core.Model;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Sixeyed.GoingAsync.Core.Incoming
{
    public class IncomingTradeFileWatcher : IDisposable
    {
        private FileSystemWatcher _watcher;
        private IIncomingTradeProcessor _processor;
        private ITradeContextFactory _dbFactory;

        public IncomingTradeFileWatcher(IIncomingTradeProcessor processor, ITradeContextFactory dbFactory, string path)
        {
            _processor = processor;
            _dbFactory = dbFactory;
            _watcher = new FileSystemWatcher(path);
            _watcher.Filter = "*.xml";
            _watcher.Created += OnFileCreated;     
        }

        public void Start(bool processExistingFiles = false)
        {
            if (processExistingFiles)
            {
                ProcessExistingFiles();
            }

            _watcher.EnableRaisingEvents = true;
        }

        private void ProcessExistingFiles()
        {
            var files = Directory.GetFiles(_watcher.Path, _watcher.Filter);
            foreach (var file in files)
            {
                ProcessFile(file);
            }            
        }

        protected virtual void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(e.FullPath))
                return;

            Retry(e, x =>
            {
                using (new FileStream(x.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    //do nothing, if we can open the file, the task can process it
                }
            }, x => ProcessFile(x.FullPath), 10);
        }

        private void ProcessFile(string path)
        {
            //replace the first line to remove XML encoding, which SQL doesn't like:
            var xml = File.ReadAllLines(path);
            xml[0] = @"<?xml version=""1.0""?>";
            
            var incoming = new IncomingTrade
            {
                ReceivedAt = DateTime.UtcNow,
                Fpml = string.Concat(xml)
            };

            var xdoc = XDocument.Parse(incoming.Fpml);
            var nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("fpml", "http://www.fpml.org/FpML-5/reporting");

            incoming.MessageId = xdoc.XPathSelectElement(XPathQuery.MessageId, nsmgr).Value;
            incoming.Party1Lei = xdoc.XPathSelectElement(XPathQuery.Party1Lei, nsmgr).Value;
            incoming.Party2Lei = xdoc.XPathSelectElement(XPathQuery.Party2Lei, nsmgr).Value;

            using (var db = _dbFactory.GetContext())
            {
                db.IncomingTrades.Add(incoming);
                db.SaveChanges();
            }

            _processor.Process(incoming);

            File.Delete(path);
        }

        private struct XPathQuery
        {
            public const string MessageId = "//fpml:nonpublicExecutionReport/fpml:header/fpml:messageId";
            public const string Party1Lei = "//fpml:nonpublicExecutionReport/fpml:party[@id='party1']/fpml:partyId";
            public const string Party2Lei = "//fpml:nonpublicExecutionReport/fpml:party[@id='party2']/fpml:partyId";
        }

        public static void Retry<T>(T input, Action<T> action, Action<T> successAction, int attempts, int waitIntervalMilliseconds = 100)
        {
            var attempt = 1;
            while (attempt <= attempts)
            {
                try
                {
                    action(input);
                    successAction(input);
                    return;
                }
                catch
                {
                    if (attempt < attempts)
                    {
                        attempt++;
                        Thread.Sleep(waitIntervalMilliseconds);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_watcher != null)
                {
                    _watcher.EnableRaisingEvents = false;
                    _watcher.Dispose();
                }
                _processor.Dispose();
            }
        }
    }
}

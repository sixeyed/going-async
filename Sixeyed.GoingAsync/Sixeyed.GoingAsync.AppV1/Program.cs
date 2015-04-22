using Microsoft.Practices.Unity;
using Sixeyed.GoingAsync.Core;
using Sixeyed.GoingAsync.Core.Incoming;
using Sixeyed.GoingAsync.Core.Model;
using System;

namespace Sixeyed.GoingAsync.AppV1
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = Args.Configuration.Configure<Arguments>().CreateAndBind(args);

            var container = new UnityContainer();
            container.RegisterType<ITradeContextFactory, TradeContextFactory>();

            switch (arguments.Version)
            {
                case "1.0":
                    container.RegisterType<IIncomingTradeProcessor, SynchronousTradeProcessor>();
                    break;
                case "1.1":
                    container.RegisterType<IIncomingTradeProcessor, TaskTradeProcessor>();
                    break;
                case "1.2":
                    container.RegisterType<IIncomingTradeProcessor, ActionBlockTradeProcessor>();
                    break;
            }

            var path = Config.Get("FileWatcher.Path");
            container.RegisterType<IncomingTradeFileWatcher>(new InjectionConstructor(
                                                                new ResolvedParameter<IIncomingTradeProcessor>(),
                                                                new ResolvedParameter<ITradeContextFactory>(),
                                                                path));

            var watcher = container.Resolve<IncomingTradeFileWatcher>();
            watcher.Start(true);
            Console.WriteLine(" ** v" + arguments.Version + " IncomingTradeFileWatcher listening (Enter to exit) **");
            Console.ReadLine();
            watcher.Dispose();
        }
    }
}

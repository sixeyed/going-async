using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.GoingAsync.Core.Incoming;
using Moq;
using System.IO;
using Sixeyed.GoingAsync.Core.Model;
using System.Data.Entity;
using System.Threading;
namespace Sixeyed.GoingAsync.Core.Tests.Incoming
{
    [TestClass]
    [DeploymentItem("Resources", "Resources")]
    [DeploymentItem("EntityFramework.SqlServer.dll")]
    public class FileWatcherTests
    {
        [TestMethod]
        public void OnFileCreatedCallsProcessor()
        {
            var incomingTradesMock = new Mock<DbSet<IncomingTrade>>();
            var trades = new List<IncomingTrade>();            
            incomingTradesMock.SetupData(trades);

            var dbMock = new Mock<TradeContext>();
            dbMock.SetupGet(x => x.IncomingTrades).Returns(incomingTradesMock.Object);
            
            var dbFactoryMock = new Mock<ITradeContextFactory>();
            dbFactoryMock.Setup(x => x.GetContext()).Returns(dbMock.Object);
            
            var processorMock = new Mock<IIncomingTradeProcessor>();

            var tempPath = Path.GetTempPath();
            var fpml = File.ReadAllText(@"Resources\Fpml-Valid_5-7.xml");
            var outPath = Path.Combine(tempPath, Guid.NewGuid().ToString().Substring(0, 4) + ".xml");

            using (var fileWatcher = new IncomingTradeFileWatcher(processorMock.Object, dbFactoryMock.Object, tempPath))
            {
                fileWatcher.Start();
                File.WriteAllText(outPath, fpml);
            }

            Thread.Sleep(2500);

            Assert.IsFalse(File.Exists(outPath));
            Assert.AreEqual(1, trades.Count);

            incomingTradesMock.Verify(x => x.Add(It.Is<IncomingTrade>(t => t.MessageId == "BANKX0001")), Times.Once);
            dbMock.Verify(x => x.SaveChanges(), Times.Once);
            processorMock.Verify(x => x.Process(It.Is<IncomingTrade>(t => t.MessageId == "BANKX0001")), Times.Once);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.GoingAsync.Steps.Enrich;

namespace Sixeyed.GoingAsync.Steps.Tests.Enrich
{
    [TestClass]
    public class PartyEnricherTests
    {
        [TestMethod]
        public void KnownLeiReturnsInternalId()
        {
            var lei = "549300O5MFEP1XJ40B46";
            var enricher = new PartyEnricher();
            var internalId = enricher.GetInternalId(lei);
            Assert.AreEqual("5678", internalId);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void UnknownLeiThrowsException()
        {
            var lei = "xyz";
            var enricher = new PartyEnricher();
            enricher.GetInternalId(lei);            
        }
    }
}

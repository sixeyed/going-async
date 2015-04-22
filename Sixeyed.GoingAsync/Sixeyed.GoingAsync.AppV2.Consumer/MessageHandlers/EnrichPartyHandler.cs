using Sixeyed.GoingAsync.Core.Messages;
using Sixeyed.GoingAsync.Core.Model;
using Sixeyed.GoingAsync.Steps.Enrich;
using System;
using System.Diagnostics;

namespace Sixeyed.GoingAsync.AppV2.Consumer.MessageHandlers
{
    public class EnrichPartyHandler : IMessageHandler
    {
        private readonly ITradeContextFactory _dbFactory;
        private readonly bool _enrichParty1;
        private readonly bool _enrichParty2;

        public EnrichPartyHandler(ITradeContextFactory dbFactory, bool enrichParty1, bool enrichParty2)
        {
            _dbFactory = dbFactory;
            _enrichParty1 = enrichParty1;
            _enrichParty2 = enrichParty2;
        }

        public bool IsHandled(string messageType)
        {
            return messageType.Equals("EnrichParty1Message", StringComparison.InvariantCultureIgnoreCase) ||
                   messageType.Equals("EnrichParty2Message", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Handle(IMessage message)
        {
            var stopwatch = Stopwatch.StartNew();
            var tradeMessage = message as ValidateTradeMessage;

            Console.WriteLine("| EnrichPartyHandler | Received trade with message ID: " + tradeMessage.TradeId);

            using (var db = _dbFactory.GetContext())
            {
                var trade = db.IncomingTrades.Find(tradeMessage.TradeId);

                var enricher = new PartyEnricher();
                if (_enrichParty1)
                {
                    trade.Party1Id = enricher.GetInternalId(trade.Party1Lei);
                }
                if (_enrichParty2)
                {
                    trade.Party2Id = enricher.GetInternalId(trade.Party2Lei);
                }

                trade.ProcessedAt = DateTime.UtcNow;
                db.SaveChanges();
            }

            Console.WriteLine("* | EnrichPartyHandler | Processed trade with ID: {0}, took: {1}ms", tradeMessage.TradeId, stopwatch.ElapsedMilliseconds);
        }
    }
}
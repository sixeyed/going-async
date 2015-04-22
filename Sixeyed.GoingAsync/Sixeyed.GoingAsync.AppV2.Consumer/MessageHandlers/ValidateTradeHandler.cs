using Sixeyed.GoingAsync.Core.Messages;
using Sixeyed.GoingAsync.Core.Model;
using Sixeyed.GoingAsync.Steps.Validate;
using System;
using System.Diagnostics;
using System.Linq;

namespace Sixeyed.GoingAsync.AppV2.Consumer.MessageHandlers
{
    public class ValidateTradeHandler : IMessageHandler
    {
        protected ITradeContextFactory _dbFactory;

        public ValidateTradeHandler(ITradeContextFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public bool IsHandled(string messageType)
        {
            return messageType.Equals("ValidateTradeMessage", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Handle(IMessage message)
        {
            var stopwatch = Stopwatch.StartNew();
            var tradeMessage = message as ValidateTradeMessage;

            Console.WriteLine("| ValidateTradeHandler | Received trade with message ID: " + tradeMessage.TradeId);

            using (var db = _dbFactory.GetContext())
            {
                var trade = db.IncomingTrades.Find(tradeMessage.TradeId);

                var validator = new FpmlValidator();
                var failures = validator.Validate(trade.Fpml);
                trade.IsFpmlValid = failures.Any() == false;

                trade.ProcessedAt = DateTime.UtcNow;
                db.SaveChanges();
            }

            Console.WriteLine("* | ValidateTradeHandler | Processed trade with ID: {0}, took: {1}ms", tradeMessage.TradeId, stopwatch.ElapsedMilliseconds);
        }
    }
}
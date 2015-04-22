using Sixeyed.GoingAsync.Core.Incoming;
using Sixeyed.GoingAsync.Core.Model;
using Sixeyed.GoingAsync.Steps.Enrich;
using Sixeyed.GoingAsync.Steps.Validate;
using System;
using System.Diagnostics;
using System.Linq;

namespace Sixeyed.GoingAsync.AppV1
{
    public abstract class TradeProcessorBase : IIncomingTradeProcessor
    {
       public abstract void Process(IncomingTrade trade);

       protected ITradeContextFactory _dbFactory;

       public TradeProcessorBase(ITradeContextFactory dbFactory)
       {
           _dbFactory = dbFactory;
       }
        
       protected void ProcessInternal(IncomingTrade incomingTrade)
       {
           try
           {
               Console.WriteLine("Received trade with message ID: " + incomingTrade.Id);
               var stopwatch = Stopwatch.StartNew();

               using (var db = _dbFactory.GetContext())
               {
                   var trade = db.IncomingTrades.Find(incomingTrade.Id);

                   //validate:
                   var validator = new FpmlValidator();
                   var failures = validator.Validate(trade.Fpml);
                   trade.IsFpmlValid = failures.Any() == false;

                   //enrich:
                   if (trade.IsFpmlValid.Value)
                   {
                       var enricher = new PartyEnricher();
                       trade.Party1Id = enricher.GetInternalId(trade.Party1Lei);
                       trade.Party2Id = enricher.GetInternalId(trade.Party2Lei);
                   }

                   //TODO - transform, route, operate

                   trade.ProcessedAt = DateTime.UtcNow;
                   db.SaveChanges();
               }

               Console.WriteLine("* Processed trade with ID: {0}, took: {1}ms", incomingTrade.Id, stopwatch.ElapsedMilliseconds);
           }
           catch (Exception ex)
           {
               Console.WriteLine("*** PROCESSING FAILED, trade ID: {0}, ex: {1}", incomingTrade.Id, ex);
           }
       }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //nothing
        }
    }
}

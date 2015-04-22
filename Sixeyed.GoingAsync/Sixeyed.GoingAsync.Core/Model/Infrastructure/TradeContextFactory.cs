using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.Core.Model
{
    public class TradeContextFactory : ITradeContextFactory
    {
        private static string _ConnectionString;
    
        static TradeContextFactory()
        {
            _ConnectionString = Config.Get("TradeContext.ConnectionString");
            //warm up the EF context:
            using(var db = new TradeContext(_ConnectionString))
            {
                var dummy = db.IncomingTrades.Count();
            }
        }

        public TradeContext GetContext()
        {            
            return new TradeContext(_ConnectionString);
        }
    }
}

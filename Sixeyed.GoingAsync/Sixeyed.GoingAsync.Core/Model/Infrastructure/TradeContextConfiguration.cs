using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace Sixeyed.GoingAsync.Core.Model
{
    public class TradeContextConfiguration : DbConfiguration
    {
        public TradeContextConfiguration()
        {
            Console.WriteLine(" *** TradeContextConfiguration *** setting SqlAzureExecutionStrategy");
            this.SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy(10, TimeSpan.FromSeconds(5)));
        }
    }
}
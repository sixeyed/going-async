using System;
using System.Data.Entity;
using System.Linq;

namespace Sixeyed.GoingAsync.Core.Model
{
    public class TradeContext : DbContext
    {
        public TradeContext() { }

        public TradeContext(string connectionString) : base(connectionString) { }

        public virtual DbSet<IncomingTrade> IncomingTrades { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
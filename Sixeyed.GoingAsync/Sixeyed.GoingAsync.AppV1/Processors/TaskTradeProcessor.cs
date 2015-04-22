using Sixeyed.GoingAsync.Core.Model;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.AppV1
{
    public class TaskTradeProcessor : TradeProcessorBase
    {
        public TaskTradeProcessor(ITradeContextFactory dbFactory) : base(dbFactory) { }

        public override void Process(IncomingTrade incomingTrade)
        {
            Task.Factory.StartNew(() => ProcessInternal(incomingTrade));
        }
    }
}

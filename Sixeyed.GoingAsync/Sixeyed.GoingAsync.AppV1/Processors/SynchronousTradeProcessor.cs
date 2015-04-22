using Sixeyed.GoingAsync.Core.Model;

namespace Sixeyed.GoingAsync.AppV1
{
    public class SynchronousTradeProcessor : TradeProcessorBase
    {
        public SynchronousTradeProcessor(ITradeContextFactory dbFactory) : base(dbFactory) { }

        public override void Process(IncomingTrade incomingTrade)
        {
            ProcessInternal(incomingTrade);
        }
    }
}

using Sixeyed.GoingAsync.Core.Model;
using System.Threading.Tasks.Dataflow;

namespace Sixeyed.GoingAsync.AppV1
{
    public class ActionBlockTradeProcessor : TradeProcessorBase
    {
        private readonly ActionBlock<IncomingTrade> _processBlock;

        public ActionBlockTradeProcessor(ITradeContextFactory dbFactory) : base(dbFactory)
        {
            _processBlock = new ActionBlock<IncomingTrade>(trade => ProcessInternal(trade));
        }

        public override void Process(IncomingTrade incomingTrade)
        {
            _processBlock.Post(incomingTrade);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _processBlock.Complete();
                _processBlock.Completion.Wait();                
            }
        }
    }
}

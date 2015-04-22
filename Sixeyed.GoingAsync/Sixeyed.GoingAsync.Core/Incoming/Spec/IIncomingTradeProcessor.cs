using Sixeyed.GoingAsync.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.Core.Incoming
{
    public interface IIncomingTradeProcessor : IDisposable
    {
        void Process(IncomingTrade trade);
    }
}

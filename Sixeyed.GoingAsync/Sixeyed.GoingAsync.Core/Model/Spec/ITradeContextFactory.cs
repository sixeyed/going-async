using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.Core.Model
{
    public interface ITradeContextFactory
    {
        TradeContext GetContext();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.Core.Messages
{
    public class ValidateTradeMessage : IMessage
    {
        public int TradeId { get; set; }
    }
}

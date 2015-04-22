using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.Core.Messages
{
    public class EnrichParty2Message : IMessage
    {
        public int TradeId { get; set; }

        public string Party2Lei { get; set; }
    }
}

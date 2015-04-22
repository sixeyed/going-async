using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.Tools.TradeGenerator
{
    public class TradeGeneratorArguments
    {
        [DefaultValue(1000)]
        public int Count { get; set; }

        [DefaultValue("v1")]
        public string AppVersion { get; set; }
    }
}

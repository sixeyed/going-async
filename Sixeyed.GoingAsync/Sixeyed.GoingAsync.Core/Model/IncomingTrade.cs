using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.Core.Model
{
    public class IncomingTrade
    {
        [Key]
        public int Id { get; set; }

        public string MessageId { get; set; }

        public DateTime ReceivedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string Party1Lei { get; set; }

        public string Party2Lei { get; set; }

        public string Party1Id { get; set; }

        public string Party2Id { get; set; }

        public string Fpml { get; set; }

        public bool? IsFpmlValid { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tfo.Finance.Domain.Entities
{
    public class Quote
    {
        public DateTime UpdateTime { get; set; }
        public decimal? Bid { get; set; }
        public decimal? Ask { get; set; }
        public decimal? Last { get; set; }
    }
}

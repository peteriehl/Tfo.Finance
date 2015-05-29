using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tfo.Finance.Domain.Abstract;

namespace Tfo.Finance.Domain.Entities
{
    public class Option : SecurityBase
    {
        public decimal Strike { get; set; }
        public DateTime Expiry { get; set; }
        public PutCall PutCall { get; set; }

        public override void Update(Quote newQuote)
        {
            Quote = newQuote;
        }

        public override string ToString()
        {
            return Symbol;
        }
    }
}

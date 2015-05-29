using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tfo.Finance.Domain.Abstract;

namespace Tfo.Finance.Domain.Entities
{
    public class Stock : SecurityBase
    {
        public override void Update(Quote newQuote)
        {
            Quote = newQuote;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tfo.Finance.Domain.Entities;

namespace Tfo.Finance.Domain.Abstract
{
    public interface ISecurity
    {
        string Symbol { get; }
        Quote Quote { get; }
        void Update(Quote newQuote);
    }

    public abstract class SecurityBase : ISecurity
    {
        public string Symbol { get; set; }
        public string Description { get; internal set; }
        public Quote Quote { get; internal set; }

        public abstract void Update(Quote newQuote);
    }
}

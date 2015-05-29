using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tfo.Finance.Domain.Abstract
{
    public interface ISecurityQuoteProvider
    {
        SecurityBase GetSecurityQuote(string symbol);
        void SubscribeToQuoteUpdates(SecurityBase security);
    }
}

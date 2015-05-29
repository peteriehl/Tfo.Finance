using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tfo.Finance.Domain.Entities;

namespace Tfo.Finance.Domain.Abstract
{
    public interface IIssuerProvider
    {
        Issuer GetIssuer(string stockSymbol);
    }
}

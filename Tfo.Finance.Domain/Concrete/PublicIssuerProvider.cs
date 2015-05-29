using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tfo.Finance.Domain.Abstract;
using Tfo.Finance.Domain.Entities;

namespace Tfo.Finance.Domain.Concrete
{
    public class PublicIssuerProvider : IIssuerProvider
    {
        YahooStockProvider _stockProvider;
        GoogleOptionProvider _optionProvider;

        public PublicIssuerProvider()
        {
            _stockProvider = new YahooStockProvider();
            _optionProvider = new GoogleOptionProvider();
        }

        public Issuer GetIssuer(string stockSymbol)
        {
            var stock = _stockProvider.GetSecurity(stockSymbol);
            var options = _optionProvider.GetOptionChain(stockSymbol);

            Issuer retVal = new Issuer(stock.Description, options.Union(new[] { stock }));
            return retVal;
        }
    }
}

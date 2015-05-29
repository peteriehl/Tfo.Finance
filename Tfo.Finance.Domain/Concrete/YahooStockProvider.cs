using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tfo.Finance.Domain.Abstract;
using Tfo.Finance.Domain.Entities;

namespace Tfo.Finance.Domain.Concrete
{
    public class YahooStockProvider
    {
        private const string BASE_QUOTE = "select * from yahoo.finance.quotes where symbol in ('{0}')";
        private const string BASE_URL = "http://query.yahooapis.com/v1/public/yql?q={0}&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

        public SecurityBase GetSecurity(string symbol)
        {
            var queryString = string.Format(BASE_QUOTE, symbol).Replace(" ", "%20").Replace("'", "%22");
            var uri = string.Format(BASE_URL, queryString);

            XDocument response = XDocument.Load(uri);

            return ParseStock(symbol, response);
        }

        private static Stock ParseStock(string symbol, XDocument response)
        {
            try
            {
                XElement data = response.Root.Element("results").Elements("quote").FirstOrDefault();

                Stock s = new Stock()
                {
                    Symbol = symbol,
                    Description = data.Element("Name").Value,
                    Quote = new Quote()
                    {
                        Ask = data.Element("Ask").Value == string.Empty ? Convert.ToDecimal(data.Element("AskRealtime").Value) : Convert.ToDecimal(data.Element("Ask").Value),
                        Bid = data.Element("Bid").Value == string.Empty ? Convert.ToDecimal(data.Element("BidRealtime").Value) : Convert.ToDecimal(data.Element("Bid").Value),
                        Last = Convert.ToDecimal(data.Element("LastTradePriceOnly").Value),
                        UpdateTime = DateTime.Now
                    }
                };

                return s;
            }
            catch (Exception ex)
            {
                // logging here.
            }

            return null;
        }
    }
}

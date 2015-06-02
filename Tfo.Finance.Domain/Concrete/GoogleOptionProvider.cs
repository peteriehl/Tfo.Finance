using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Tfo.Finance.Domain.Entities;

namespace Tfo.Finance.Domain.Concrete
{
    public class GoogleOptionProvider
    {
        private const string MOST_RECENT_REQUEST_URL = @"http://www.google.com/finance/option_chain?q={0}&type=All&output=json";
        private const string SPECIFIC_EXPIRTY_REQUEST_URL = @"http://www.google.com/finance/option_chain?cid={0}&expd={1}&expm={2}&expy={3}&output=json";

        public List<Option> GetOptionChain(string symbol)
        {
            WebRequest webRequest = WebRequest.Create(string.Format(MOST_RECENT_REQUEST_URL, symbol));
            var response = webRequest.GetResponse();

            List<DateTime> expirations;
            string underlyingCid;

            List<Option> retVal = new List<Option>();

            using (Stream responseStream = response.GetResponseStream())
            {
                retVal.AddRange(ParseResponse(responseStream, out expirations, out underlyingCid));
            }

            List<DateTime> extraExpiries;
            string extraUnderlyingCids;

            foreach (var expiry in expirations)
            {
                webRequest = WebRequest.Create(string.Format(SPECIFIC_EXPIRTY_REQUEST_URL, underlyingCid, expiry.Day, expiry.Month, expiry.Year));
                response = webRequest.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    retVal.AddRange(ParseResponse(responseStream, out extraExpiries, out extraUnderlyingCids));
                }
            }

            return retVal;
        }

        public Option GetSecurityQuote(string symbol)
        {
            throw new NotImplementedException();
        }

        private List<Option> ParseResponse(Stream responseStream, out List<DateTime> expirations, out string underlyingCid)
        {
            using (StreamReader reader = new StreamReader(responseStream))
            {
                try
                {
                    string responseString = reader.ReadToEnd();
                    JObject jsonCollection = JObject.Parse(responseString);

                    decimal? nullDecimal = null;

                    expirations =
                        jsonCollection["expirations"]
                        .Select(e => new DateTime(int.Parse(e["y"].ToString()), int.Parse(e["m"].ToString()), int.Parse(e["d"].ToString())))
                        .ToList();

                    if (expirations.Any())
                        expirations.Remove(expirations.First());

                    underlyingCid = jsonCollection["underlying_id"].ToString();

                    var retVal =
                        jsonCollection["puts"].Children()
                        .Select(e => new Option()
                        {
                            Symbol = e["s"].ToString(),
                            Expiry = DateTime.ParseExact(e["expiry"].ToString(), "MMM d, yyyy", null),
                            Strike = e["strike"].Value<decimal>(),
                            PutCall = PutCall.Put,
                            Quote = new Quote()
                            {
                                Bid = e["b"].ToString() == "-" ? nullDecimal : Convert.ToDecimal(e["b"].ToString()),
                                Ask = e["a"].ToString() == "-" ? nullDecimal : Convert.ToDecimal(e["a"].ToString()),
                                Last = e["p"].ToString() == "-" ? nullDecimal : Convert.ToDecimal(e["p"].ToString()),
                                UpdateTime = DateTime.Now
                            }
                        })
                        .Union(jsonCollection["calls"].Children()
                        .Select(e => new Option()
                        {
                            Symbol = e["s"].ToString(),
                            Expiry = DateTime.ParseExact(e["expiry"].ToString(), "MMM d, yyyy", null),
                            Strike = e["strike"].Value<decimal>(),
                            PutCall = PutCall.Call,
                            Quote = new Quote()
                            {
                                Bid = e["b"].ToString() == "-" ? nullDecimal : Convert.ToDecimal(e["b"].ToString()),
                                Ask = e["a"].ToString() == "-" ? nullDecimal : Convert.ToDecimal(e["a"].ToString()),
                                Last = e["p"].ToString() == "-" ? nullDecimal : Convert.ToDecimal(e["p"].ToString()),
                                UpdateTime = DateTime.Now
                            }
                        }))
                        .ToList();

                    return retVal;
                }
                catch (Exception ex)
                {
                    // log
                }
            }

            expirations = null;
            underlyingCid = null;
            return new List<Option>();
        }
    }
}

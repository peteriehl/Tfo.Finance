using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tfo.Finance.Domain.Abstract;
using Tfo.Finance.Domain.Entities;

namespace Tfo.Finance.Web.Controllers
{
    public class IssuerController : Controller
    {
        private IIssuerProvider _issuerProvider;

        public IssuerController(IIssuerProvider issuerProvider)
        {
            _issuerProvider = issuerProvider;
        }

        // GET: Issuer
        public ActionResult Search()
        {
            return View(new Stock());
        }

        [HttpPost]
        public ActionResult Display(Stock stock)
        {
            var issuer = _issuerProvider.GetIssuer(stock.Symbol.ToUpper());
            return View(issuer);
        }
    }
}
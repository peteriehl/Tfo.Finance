using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tfo.Finance.Domain.Abstract;

namespace Tfo.Finance.Domain.Entities
{
    public class Issuer
    {
        public string Description { get; private set; }

        public List<SecurityBase> Securities { get; private set; }

        public Issuer(string description, IEnumerable<SecurityBase> securities)
        {
            Description = description;
            Securities = securities.ToList();
        }
    }
}

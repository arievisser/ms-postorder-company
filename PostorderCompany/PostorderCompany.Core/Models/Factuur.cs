using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostorderCompany.Core.Models
{
    public class Factuur
    {
        public string orderId { get; set; }
        public string betaalMethode { get; set; }
        public bool afgehandeld { get; set; }

    }
}

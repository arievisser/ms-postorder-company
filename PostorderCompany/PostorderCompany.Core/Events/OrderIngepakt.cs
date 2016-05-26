using PostorderCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostorderCompany.Core.Events
{
    public class OrderIngepakt : BaseEvent
    {
        public string orderId { get; set; }
        public string gewicht { get; set; }
        public string afmetingen { get; set; }
    }
}

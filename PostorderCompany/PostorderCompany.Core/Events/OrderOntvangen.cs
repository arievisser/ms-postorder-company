using PostorderCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostorderCompany.Core.Events
{
    public class OrderOntvangen : BaseEvent
    {
        public string orderId;
        public Persoonsgegevens klant;
        public List<OrderItem> items;
    }
}

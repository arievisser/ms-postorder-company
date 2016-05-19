using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostorderCompany.Core.Models
{
    public class Order
    {
        public Persoonsgegevens klant;
        public List<OrderItem> items;
    }
}

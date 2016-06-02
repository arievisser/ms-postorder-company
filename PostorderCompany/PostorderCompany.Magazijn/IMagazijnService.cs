using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostorderCompany.Core.Events;

namespace PostorderCompany.Magazijn
{
    public interface IMagazijnService
    {
        bool HandleEvent(string eventType, string eventData);
        void sendOrder(OrderIngepakt order);
        List<OrderIngepakt> GetOrders();
        void StartListener();
    }
}
using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostorderCompany.Order
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventHandler = new RabbitMQEventHandler("PostorderCompany.Order", HandleEvent);
            eventHandler.Start();

            Console.WriteLine("*** Order Service ***\n");
            Console.ReadKey(true);

            eventHandler.Stop();
        }

        private static bool HandleEvent(string eventType, string eventData)
        {
            bool handled = true;
            switch (eventType)
            {
                case "OrderOntvangen":
                    OrderOntvangen orderOntvangen = JsonConvert.DeserializeObject<OrderOntvangen>(eventData);
                    handled = Handle(orderOntvangen);
                    break;
                case "OrderIngepakt":
                    OrderIngepakt orderIngepakt = JsonConvert.DeserializeObject<OrderIngepakt>(eventData);
                    handled = Handle(orderIngepakt);
                    break;
                case "OrderBetaald":
                    OrderBetaald orderBetaald = JsonConvert.DeserializeObject<OrderBetaald>(eventData);
                    handled = Handle(orderBetaald);
                    break;
            }

            return handled;
        }

        private static bool Handle(OrderOntvangen orderOntvangen)
        {
            Console.WriteLine("Order Ontvangen, ID: " + orderOntvangen.orderId);
            return true;
        }

        private static bool Handle(OrderIngepakt orderIngepakt)
        {
            Console.WriteLine("Order Ingepakt, ID: " + orderIngepakt.orderId);
            return true;
        }

        private static bool Handle(OrderBetaald orderBetaald)
        {
            Console.WriteLine("Order Betaald, ID: " + orderBetaald.orderId);
            return true;
        }

    }
}

using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostorderCompany.Pakket
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventHandler = new RabbitMQEventHandler("PostorderCompany.Pakket", HandleEvent);
            eventHandler.Start();

            Console.WriteLine("*** Pakket Service ***\n");
            Console.ReadKey(true);

            eventHandler.Stop();
        }

        private static bool HandleEvent(string eventType, string eventData)
        {
            bool handled = true;
            switch (eventType)
            {
                case "OrderVerzonden":
                    OrderVerzonden orderVerzonden = JsonConvert.DeserializeObject<OrderVerzonden>(eventData);
                    handled = Handle(orderVerzonden);
                    break;
            }

            return handled;
        }

        private static bool Handle(OrderVerzonden orderVerzonden)
        {
            var pakketOntvangen = new PakketGereed()
            {
                routingKey = "Pakket.Gereed",
                pakketId = Guid.NewGuid().ToString("D"),
                orderId = orderVerzonden.orderId,
                afzender = orderVerzonden.afzender,
                ontvanger = orderVerzonden.ontvanger,
                gewicht = orderVerzonden.gewicht,
                afmetingen = orderVerzonden.afmetingen
            };

            new RabbitMQEventPublisher().PublishEvent(pakketOntvangen);

            return true; 
        }

    }
}

using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;
using PostorderCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostorderCompany.Order
{
    class Program
    {
        class OrderStatus
        {
            public string id;
            public Persoonsgegevens klant;
            public bool betaald;
            public bool ingepakt;
            public string gewicht;
            public string afmetingen;
        }

        static List<OrderStatus> orders;

        static void Main(string[] args)
        {
            var eventHandler = new RabbitMQEventHandler("PostorderCompany.Order", HandleEvent);
            eventHandler.Start();

            orders = new List<OrderStatus>();

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
            orders.Add(new OrderStatus()
            {
                id = orderOntvangen.orderId,
                klant = orderOntvangen.klant,
                betaald = false,
                ingepakt = false
            });

            return true;
        }

        private static bool Handle(OrderIngepakt orderIngepakt)
        {
            foreach (OrderStatus order in orders)
            {
                if (order.id.Equals(orderIngepakt.orderId))
                {
                    order.ingepakt = true;
                    order.gewicht = orderIngepakt.gewicht;
                    order.afmetingen = orderIngepakt.afmetingen;
                    verzendenWanneerBetaaldEnIngepakt(order);
                    break;
                }
            }

            return true;
        }

        private static bool Handle(OrderBetaald orderBetaald)
        {
            foreach (OrderStatus order in orders)
            {
                if (order.id.Equals(orderBetaald.orderId))
                {
                    order.betaald = true;
                    verzendenWanneerBetaaldEnIngepakt(order);
                    break;
                }
            }

            return true;
        }

        private static void verzendenWanneerBetaaldEnIngepakt(OrderStatus order) 
        {
            if (order.betaald && order.ingepakt)
            {
                var orderVerzonden = new OrderVerzonden()
                {
                    routingKey = "Order.Verzonden",
                    orderId = order.id,
                    ontvanger = order.klant,
                    afzender = new Persoonsgegevens()
                    {
                        naam = "Postorder Company",
                        emailadres = "info@postorder-company.com",
                        adres = new Adres()
                        {
                            straat = "Singel",
                            huisnummer = "1a",
                            postcode = "1234AB",
                            plaats = "Amsterdam",
                            land = "Nederland"
                        }
                    },
                    gewicht = order.gewicht,
                    afmetingen = order.afmetingen
                };

                new RabbitMQEventPublisher().PublishEvent(orderVerzonden);
            }
        }

    }
}

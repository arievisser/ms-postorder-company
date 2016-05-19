using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;
using PostorderCompany.Core.Models;

namespace PostorderCompany.Magazijn
{
    class Program {

        static void Main(string[] args) {

            Application.Run(new MagazijnForm());

            Boolean stop = false;
            var eventHandler = new RabbitMQEventHandler("PostorderCompany.Magazijn", HandleEvent);
            eventHandler.Start();

            Console.WriteLine("*** Magazijn Service ***\n");


            List<OrderItem> items = new List<OrderItem>();
            items.Add(new OrderItem {
                aantal = 1,
                artikelId = "EersteArtikelId"
            });
        
            Persoonsgegevens klant = new Persoonsgegevens {
                adres = new Adres {
                    huisnummer = "121",
                    plaats = "Nimma",
                    land = "Nederland",
                    postcode = "6546 ES",
                    straat = "hoppaaaaStraat"
                },
                emailadres = "clintje@hotmail.com",
                naam = "Clint Geense"
            };

        var orderOntvangen = new OrderOntvangen {
                orderId = "1001010101",
                items = items,
                klant = klant
            };

            Handle(orderOntvangen);
        }

        private static bool HandleEvent(string eventType, string eventData) {
            bool handled = true;
            switch (eventType) {
                case "OrderOntvangen":
                    OrderOntvangen orderOntvangen = JsonConvert.DeserializeObject<OrderOntvangen>(eventData);
                    handled = Handle(orderOntvangen);
                    break;
            }
            return handled;
        }

        private static bool Handle(OrderOntvangen orderOntvangen) {

            Console.WriteLine("Order met ordernummer {0} is ontvangen", orderOntvangen.orderId);

            //Ga pakket inpakken voor order
            Console.WriteLine("Wat is de afmeting van het pakket?");

            var afmeting = Console.ReadLine();


            Console.WriteLine("Wat is het gewicht van het pakket?");

            var gewicht = Console.ReadLine();


            Console.WriteLine("Druk op Enter om pakket in te pakken en klaar te zetten voor versturen");
            Console.ReadKey(true);


            var orderIngepakt = new OrderIngepakt {
                routingKey = "Order.Ingepakt",
                orderId = orderOntvangen.orderId,
                gewicht = null,
                afmetingen = null
            };

            new RabbitMQEventPublisher().PublishEvent(orderIngepakt);

            return true;
        }
    }
}


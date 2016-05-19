using System;
using PostorderCompany.Core.Infrastructure;
using PostorderCompany.Core.Events;
using Newtonsoft.Json;

namespace PostorderCompany.Chauffeur
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var eventHandler = new RabbitMQEventHandler("PostorderCompany.Chauffeur", HandleEvent);
            eventHandler.Start();

            Console.WriteLine("*** Tracking Service ***\n");
            Console.ReadKey(true);

            eventHandler.Stop();
        }

        private static bool HandleEvent(string eventType, string eventData)
        {
            var handled = true;
            switch (eventType)
            {
                case "PakketGereed":
                    var pakketOntvangen = JsonConvert.DeserializeObject<PakketGereed>(eventData);
                    handled = Handle(pakketOntvangen);
                    break;
                default:
                    handled = false;
                    break;
            }

            return handled;
        }

        private static bool Handle(PakketGereed pakketGereed)
        {
            Console.WriteLine("Pakket ontvangen van afzender:\n"
                                + "     {0}\n"
                                + "     {1} {2}, {3} {4}, {5}\n"
                                + "     Order-ID: {6}\n"
                                + "voor ontvanger:\n"
                                + "     {7}\n"
                                + "     {8} {9}, {10} {11}, {12}\n"
                                + "{13}, {14}\n"
                                + "Vanaf nu te volgen onder pakket-ID: {15}\n",
                pakketGereed.afzender.naam,
                pakketGereed.afzender.adres.straat, pakketGereed.afzender.adres.huisnummer, pakketGereed.afzender.adres.postcode, pakketGereed.afzender.adres.plaats, pakketGereed.afzender.adres.land,
                pakketGereed.orderId,
                pakketGereed.ontvanger.naam,
                pakketGereed.ontvanger.adres.straat, pakketGereed.ontvanger.adres.huisnummer, pakketGereed.ontvanger.adres.postcode, pakketGereed.ontvanger.adres.plaats, pakketGereed.ontvanger.adres.land,
                pakketGereed.gewicht, pakketGereed.afmetingen,
                pakketGereed.pakketId);
            
            var orderOnderweg = new PakketOnderweg()
            {
                routingKey = "Order.Onderweg",
                pakketId = pakketGereed.pakketId,
                chauffeur = "Henk de Steen"
            };
            new RabbitMQEventPublisher().PublishEvent(orderOnderweg);

            var pakketAfgeleverd = new PakketAfgeleverd()
            {
                routingKey = "Order.Afgeleverd",
                pakketId = pakketGereed.pakketId,
                handtekening = "Mooie handtekening"
            };
            new RabbitMQEventPublisher().PublishEvent(pakketAfgeleverd);

            return true;
        }
    }
}

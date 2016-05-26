using System;
using PostorderCompany.Core.Infrastructure;
using PostorderCompany.Core.Events;
using Newtonsoft.Json;
using PostorderCompany.Core.Models;

namespace PostorderCompany.Tracking
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventHandler = new RabbitMQEventHandler("PostorderCompany.Tracking", HandleEvent);
            eventHandler.Start();

            Console.WriteLine("*** Tracking Service ***\n");
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
                case "OrderVerzonden":
                    OrderVerzonden orderVerzonden = JsonConvert.DeserializeObject<OrderVerzonden>(eventData);
                    handled = Handle(orderVerzonden);
                    break;

                case "PakketGereed":
                    PakketGereed pakketGereed = JsonConvert.DeserializeObject<PakketGereed>(eventData);
                    handled = Handle(pakketGereed);
                    break;
                case "PakketOnderweg":
                    PakketOnderweg pakketOnderweg = JsonConvert.DeserializeObject<PakketOnderweg>(eventData);
                    handled = Handle(pakketOnderweg);
                    break;
                case "PakketAfgeleverd":
                    PakketAfgeleverd pakketAfgeleverd = JsonConvert.DeserializeObject<PakketAfgeleverd>(eventData);
                    handled = Handle(pakketAfgeleverd);
                    break;
            }
            Console.WriteLine("\n");
            return handled;
        }

        private static bool Handle(OrderOntvangen orderOntvangen)
        {
            Console.WriteLine("Order Ontvangen: {0}\n   Klant:\n     {1}\n     {2} {3}\n     {4} {5}\n     {6}\n     {7}", 
                orderOntvangen.orderId, orderOntvangen.klant.naam, 
                orderOntvangen.klant.adres.straat, orderOntvangen.klant.adres.huisnummer, 
                orderOntvangen.klant.adres.postcode, orderOntvangen.klant.adres.plaats,
                orderOntvangen.klant.adres.land, orderOntvangen.klant.emailadres);
            Console.WriteLine("   Order Items:");

            foreach (OrderItem item in orderOntvangen.items)
            {
                Console.WriteLine("     {0}x {1}", item.aantal, item.artikelId);
            }
            return true;
        }

        private static bool Handle(OrderIngepakt orderIngepakt)
        {
            Console.WriteLine("Order Ingepakt: {0}\n   Gewicht: {1}\n   Afmetingen: {2}", orderIngepakt.orderId, orderIngepakt.gewicht, orderIngepakt.afmetingen);
            return true;
        }

        private static bool Handle(OrderBetaald orderBetaald)
        {
            Console.WriteLine("Order Betaald: {0}\n   Betaalmethode: {1}", orderBetaald.orderId, orderBetaald.betaalmethode);
            return true;
        }

        private static bool Handle(OrderVerzonden orderVerzonden)
        {
            Console.WriteLine("Order Verzonden: {0}", orderVerzonden.orderId);
            return true;
        }

        private static bool Handle(PakketGereed pakketGereed)
        {
            Console.WriteLine("Pakket Gereed\n   Afzender:\n"
                                + "     {0}\n" 
                                + "     {1} {2}, {3} {4}, {5}\n"
                                + "     Order-ID: {6}\n"
                                + "   Ontvanger:\n"
                                + "     {7}\n"
                                + "     {8} {9}, {10} {11}, {12}\n"
                                + "   Eigenschappen: {13}, {14}\n"
                                + "   Vanaf nu te volgen onder pakket-ID: {15}\n", 
                pakketGereed.afzender.naam,
                pakketGereed.afzender.adres.straat, pakketGereed.afzender.adres.huisnummer, pakketGereed.afzender.adres.postcode, pakketGereed.afzender.adres.plaats, pakketGereed.afzender.adres.land,
                pakketGereed.orderId,
                pakketGereed.ontvanger.naam,
                pakketGereed.ontvanger.adres.straat, pakketGereed.ontvanger.adres.huisnummer, pakketGereed.ontvanger.adres.postcode, pakketGereed.ontvanger.adres.plaats, pakketGereed.ontvanger.adres.land,
                pakketGereed.gewicht, pakketGereed.afmetingen,
                pakketGereed.pakketId);
            return true;
        }

        private static bool Handle(PakketOnderweg pakketOnderweg)
        {
            Console.WriteLine("Pakket Onderweg: {0}\n   Chauffeur: {1}", pakketOnderweg.pakketId, pakketOnderweg.chauffeur);
            return true;
        }

        private static bool Handle(PakketAfgeleverd pakketAfgeleverd)
        {
            Console.WriteLine("Pakket Afgeleverd: {0}\n   Handtekening: {1}", pakketAfgeleverd.pakketId, pakketAfgeleverd.handtekening);
            return true;
        }

    }
}

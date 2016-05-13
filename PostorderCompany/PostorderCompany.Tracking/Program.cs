using System;
using PostorderCompany.Core.Infrastructure;
using PostorderCompany.Core.Events;
using Newtonsoft.Json;

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
                case "PakketOntvangen":
                    PakketOntvangen pakketOntvangen = JsonConvert.DeserializeObject<PakketOntvangen>(eventData);
                    handled = Handle(pakketOntvangen);
                    break;
            }

            return handled;
        }

        private static bool Handle(PakketOntvangen pakketOntvangen)
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
                pakketOntvangen.afzender.naam,
                pakketOntvangen.afzender.adres.straat, pakketOntvangen.afzender.adres.huisnummer, pakketOntvangen.afzender.adres.postcode, pakketOntvangen.afzender.adres.plaats, pakketOntvangen.afzender.adres.land,
                pakketOntvangen.orderId,
                pakketOntvangen.ontvanger.naam,
                pakketOntvangen.ontvanger.adres.straat, pakketOntvangen.ontvanger.adres.huisnummer, pakketOntvangen.ontvanger.adres.postcode, pakketOntvangen.ontvanger.adres.plaats, pakketOntvangen.ontvanger.adres.land,
                pakketOntvangen.gewicht, pakketOntvangen.afmetingen,
                pakketOntvangen.pakketId);
            return true;
        }

    }
}

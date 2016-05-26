using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;

namespace PostorderCompany.Chauffeur
{
    class ChauffeurService
    {
        private RabbitMQEventHandler _eventHandler;
        private static List<PakketStatus> _pakketjes;

        public ChauffeurService()
        {
            StartListening();
            _pakketjes = new List<PakketStatus>();


            _pakketjes.Add(new PakketStatus()
            {
                pakketId = "abc",
                afgeleverd = false,
                onderweg = false,
            });

            _pakketjes.Add(new PakketStatus()
            {
                pakketId = "asdas",
                afgeleverd = false,
                onderweg = false,
            });

            _pakketjes.Add(new PakketStatus()
            {
                pakketId = "ergerre",
                afgeleverd = false,
                onderweg = false,
            });
        }

        public List<PakketStatus> GetStatuses()
        {
            return _pakketjes;
        }

        public void SendOrder(PakketStatus status, string chauffeur)
        {
            var orderOnderweg = new PakketOnderweg()
            {
                routingKey = "Order.Onderweg",
                pakketId = status.pakketId,
                chauffeur = chauffeur
            };
            status.onderweg = true;
            status.chauffeur = chauffeur;
            new RabbitMQEventPublisher().PublishEvent(orderOnderweg);
        }

        public void OrderDeleverd(PakketStatus status, string handtekening)
        {
            if (!status.onderweg)
                return;
            
            var pakketAfgeleverd = new PakketAfgeleverd()
            {
                routingKey = "Order.Afgeleverd",
                pakketId = status.pakketId,
                handtekening = handtekening
            };
            _pakketjes.Remove(status);
            new RabbitMQEventPublisher().PublishEvent(pakketAfgeleverd);
        }

        public void StartListening()
        {
            _eventHandler = new RabbitMQEventHandler("PostorderCompany.Chauffeur", HandleEvent);
            _eventHandler.Start();

            Console.WriteLine("*** Tracking Service ***\n");
        }

        public void StopListening()
        {
            _eventHandler?.Stop();
        }

        private static bool HandleEvent(string eventType, string eventData)
        {
            switch (eventType)
            {
                case "PakketGereed":
                    var pakketOntvangen = JsonConvert.DeserializeObject<PakketGereed>(eventData);
                    return Handle(pakketOntvangen);
                default:
                    return false;
            }
        }

        private static bool Handle(PakketGereed pakketGereed)
        {
            if (pakketGereed == null)
                return false;

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

            _pakketjes.Add(new PakketStatus()
            {
                pakketId = pakketGereed.pakketId,
                afgeleverd = false,
                onderweg = false,
            });
            return true;
        }
    }
}
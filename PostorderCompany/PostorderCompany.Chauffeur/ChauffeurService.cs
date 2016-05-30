using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;

namespace PostorderCompany.Chauffeur
{
    class ChauffeurService : IChauffeurService
    {
        private RabbitMQEventHandler _eventHandler;
        private static List<PakketStatus> _pakketjes = new List<PakketStatus>();

        public ChauffeurService()
        {
            StartListening();
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

        public void OrderDelivered(PakketStatus status, string handtekening)
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

        public bool HandleEvent(string eventType, string eventData)
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

        private bool Handle(PakketGereed pakketGereed)
        {
            if (pakketGereed == null)
                return false;

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
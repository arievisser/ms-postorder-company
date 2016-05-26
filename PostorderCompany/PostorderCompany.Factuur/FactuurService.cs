using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;

namespace PostorderCompany.Factuur
{
    public class FactuurService
    {
        private List<Core.Models.Factuur> facturen = new List<Core.Models.Factuur>();

        public FactuurService()
        {
            RabbitMQEventHandler eventHandler = new RabbitMQEventHandler("PostorderCompany.Factuur", HandleEvent);
            eventHandler.Start();
        }

        public bool HandleEvent(string eventType, string eventData)
        {
            bool handled = true;
            switch (eventType)
            {
                case "OrderOntvangen":
                    OrderOntvangen pakketOntvangen = JsonConvert.DeserializeObject<OrderOntvangen>(eventData);
                    handled = Handle(pakketOntvangen);
                    break;
            }

            return handled;
        }

        private bool Handle(OrderOntvangen pakketOntvangen)
        {
            // logica
            var factuur = new Core.Models.Factuur
            {
                orderId = pakketOntvangen.orderId
            };
            this.facturen.Add(factuur);
           
            Console.Out.WriteLine(facturen);

            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factuur"></param>
        public void SendMessage(Core.Models.Factuur factuur)
        {
            var orderBetaald = new OrderBetaald
            {
                routingKey = "Order.Betaald",
                orderId = factuur.orderId,
                betaalmethode = factuur.betaalMethode
            };
            new RabbitMQEventPublisher().PublishEvent(orderBetaald);
        }

        public List<Core.Models.Factuur> GetFacturen()
        {
            return facturen;
        }

        public void RemoreFactuur(Core.Models.Factuur factuur)
        {
            facturen.Remove(factuur);
        }
    }
}

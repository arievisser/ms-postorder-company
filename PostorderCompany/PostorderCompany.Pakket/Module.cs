using Nancy;
using Nancy.ModelBinding;
using PostorderCompany.Core.Commands;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;
using System;

namespace PostorderCompany.Delivery
{
    public class Module : NancyModule
    {
        public Module()
        {
            Get["/"] = _ => "Postorder Delivery Service API Running...";

            Post["/api/pakket"] = _ =>
            {
                var order = this.Bind<Pakket>();

                var orderOntvangen = new PakketOntvangen() {
                    routingKey = "Pakket.Ontvangen",
                    pakketId = Guid.NewGuid().ToString("D"),
                    orderId = order.orderId,
                    afzender = order.afzender,
                    ontvanger = order.ontvanger,
                    gewicht = order.gewicht,
                    afmetingen = order.afmetingen
                };

                new RabbitMQEventPublisher().PublishEvent(orderOntvangen);

                return HttpStatusCode.OK;
            };
        }
    }

}
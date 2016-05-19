using Nancy;
using Nancy.ModelBinding;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;
using System;

namespace PostorderCompany.Order.UI
{
    public class Module : NancyModule
    {
        public Module()
        {
            Get["/"] = _ => "Postorder Company Order Service Running...";

            Post["/api/order"] = _ =>
            {
                var order = this.Bind<Core.Models.Order>();

                var orderOntvangen = new OrderOntvangen()
                {
                    routingKey = "Order.Ontvangen",
                    orderId = GetNewOrderId(),
                    klant = order.klant,
                    items = order.items
                };

                new RabbitMQEventPublisher().PublishEvent(orderOntvangen);

                return HttpStatusCode.OK;
            };
        }

        private string GetNewOrderId()
        {
            return Guid.NewGuid().ToString("D");
        }

    }
}
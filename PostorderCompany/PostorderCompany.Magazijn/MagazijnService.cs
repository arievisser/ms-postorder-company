using System.Collections.Generic;
using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;
using PostorderCompany.Magazijn;

public class MagazijnService : IMagazijnService
{
    private static List<OrderIngepakt> orders = new List<OrderIngepakt>();

    public MagazijnService() {
        var eventHandler = new RabbitMQEventHandler("PostorderCompany.Magazijn", HandleEvent);
        eventHandler.Start();
    }

    public void sendOrder(OrderIngepakt order) {
        new RabbitMQEventPublisher().PublishEvent(order);
        orders.Remove(order);
    }

    public List<OrderIngepakt> GetOrders() {
        return orders;
    }

    public bool HandleEvent(string eventType, string eventData) {
        bool handled = true;
        switch (eventType) {
            case "OrderOntvangen":
                OrderOntvangen orderOntvangen = JsonConvert.DeserializeObject<OrderOntvangen>(eventData);
                handled = Handle(orderOntvangen);
                break;
        }
        return handled;
    }

    private bool Handle(OrderOntvangen orderOntvangen) {

        var order = new OrderIngepakt() {
            routingKey = "Order.Ingepakt",
            orderId = orderOntvangen.orderId,
            afmetingen = null,
            gewicht = null,
        };

        orders.Add(order);

        return true;
    }
}


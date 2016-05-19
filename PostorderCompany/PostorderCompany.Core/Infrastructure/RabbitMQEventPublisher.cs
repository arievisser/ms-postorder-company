using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostorderCompany.Core.Infrastructure
{
    public class RabbitMQEventPublisher
    {
        public void PublishEvent(BaseEvent e)
        {

            var factory = new ConnectionFactory() {
                HostName = "192.168.1.5",
                UserName = "postordercompany",
                Password = "postordercompany"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    settings.NullValueHandling = NullValueHandling.Include;
                    string message = JsonConvert.SerializeObject(e, settings);
                    var body = Encoding.UTF8.GetBytes(message);
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Headers = new Dictionary<string, object>();
                    properties.Headers.Add("EventType", e.GetType().Name);
                    channel.BasicPublish("PostorderCompany", e.routingKey, properties, body);
                }
            }
        }
    }
}

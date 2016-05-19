using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace PostorderCompany.Core.Infrastructure
{
    public class RabbitMQEventHandler
    {
        private const string DEFAULT_HOST = "192.168.1.5";
        private const string DEFAULT_USERNAME = "postordercompany";
        private const string DEFAULT_PASSWORD = "postordercompany";

        private const int DEQUEUE_TIMEOUT = 2000;
        private bool _cancellationRequested = false;
        private Thread _workerThread;
        private string _host;
        private string _username;
        private string _password;
        private string _queuename;
        private Func<string, string, bool> _callback;

        public RabbitMQEventHandler(string host, string username, string password, string queuename, Func<string, string, bool> callback)
        {
            _host = host;
            _username = username;
            _password = password;
            _queuename = queuename;
            _callback = callback;
        }


        public RabbitMQEventHandler(string queuename, Func<string, string, bool> callback)
        {
            _host = DEFAULT_HOST;
            _username = DEFAULT_USERNAME;
            _password = DEFAULT_PASSWORD;
            _queuename = queuename;
            _callback = callback;
        }

        public void Start()
        {
            _workerThread = new Thread(new ThreadStart(HandleEvents));
            _workerThread.Start();
        }

        public void Stop()
        {
            _cancellationRequested = true;
            _workerThread.Join(TimeSpan.FromSeconds(5));
        }

        private void HandleEvents()
        {
            var factory = new ConnectionFactory() { HostName = _host, UserName = _username, Password = _password };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(_queuename, false, consumer);
                    while (!_cancellationRequested)
                    {
                        BasicDeliverEventArgs ea;
                        if (consumer.Queue.Dequeue(DEQUEUE_TIMEOUT, out ea))
                        {
                            if (HandleEvent(ea))
                            {
                                channel.BasicAck(ea.DeliveryTag, false);
                            }
                        }
                    }
                }
            }
        }

        private bool HandleEvent(BasicDeliverEventArgs ea)
        {
            string message = Encoding.UTF8.GetString(ea.Body);
            string eventType = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["EventType"]);
            return _callback(eventType, message);
        }
    }
}

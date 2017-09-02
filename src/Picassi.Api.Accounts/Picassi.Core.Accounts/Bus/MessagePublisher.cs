using System;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Picassi.Core.Accounts.Bus
{
    public interface IMessagePublisher
    {
        void Publish<T>(string type, T model);
    }

    public class MessagePublisher : IMessagePublisher
    {
        private readonly ConnectionFactory _factory;

        public MessagePublisher()
        {
            _factory = new ConnectionFactory
            {
                HostName = ConfigurationManager.AppSettings["rabbit-mq-host"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["rabbit-mq-port"]),
                UserName = ConfigurationManager.AppSettings["rabbit-mq-user"],
                Password = ConfigurationManager.AppSettings["rabbit-mq-password"],
                VirtualHost = "/"
            };
        }

        public void Publish<T>(string type, T model)
        {
            PublishMessage(Exchanges.Tracking, type, model);
        }

        private void PublishMessage<T>(IExchangeConfig tracking, string type, T model)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(tracking.Name, tracking.Type, tracking.Durable);

                    var jsonified = JsonConvert.SerializeObject(model);
                    var messageDetails = new MessageDetails
                    {                        
                        Payload = jsonified, Time = DateTime.UtcNow, Type = type
                    };
                    var jsonifiedMessageDetails = JsonConvert.SerializeObject(messageDetails);
                    var body = Encoding.UTF8.GetBytes(jsonifiedMessageDetails);

                    channel.BasicPublish(tracking.Name, tracking.RoutingKey, null, body);
                }
            }
        }
    }
}
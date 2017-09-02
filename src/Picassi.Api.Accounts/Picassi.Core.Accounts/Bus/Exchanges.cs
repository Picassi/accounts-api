using System.Diagnostics;
using System.Text;
using RabbitMQ.Client.Events;

namespace Picassi.Core.Accounts.Bus
{
    public class Exchanges
    {
        public static readonly IExchangeConfig Tracking = new ExchangeConfig(
            "tracking-exchange", "fanout", true, string.Empty);
    }

    public interface IExchangeConfig
    {
        string Name { get; }
        string Type { get; }
        bool Durable { get; }
        string RoutingKey { get; }
        void OnReceived(object sender, BasicDeliverEventArgs ea);
    }

    public class ExchangeConfig : IExchangeConfig
    {
        public ExchangeConfig(string name, string type, bool durable, string routingKey)
        {
            Name = name;
            Type = type;
            Durable = durable;
            RoutingKey = routingKey;
        }

        public string Name { get; }
        public string Type { get; }
        public bool Durable { get; }
        public string RoutingKey { get; }

        public void OnReceived(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Debug.WriteLine("{0}", message);
        }
    }
}
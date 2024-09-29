using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;

namespace CustomerOrder.API.Services.RabbitMq
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public RabbitMqService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "order_notifications",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                 routingKey: "order_notifications",
                                 basicProperties: null,
                                 body: body);
        }
    }
}

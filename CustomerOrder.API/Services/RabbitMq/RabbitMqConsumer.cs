using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;

namespace CustomerOrder.API.Services.RabbitMq
{
    public class RabbitMqConsumer
    {
        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "order_notifications",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    WriteMessageToFile(message);
                };

                channel.BasicConsume(queue: "order_notifications",
                                     autoAck: true,
                                     consumer: consumer);
                Console.WriteLine("Consumer started, waiting for messages.");
                Console.ReadLine();
            }
        }
        private void WriteMessageToFile(string message)
        {
            var filePath = "order_notifications.txt";
            File.AppendAllText(filePath, $"{message}{Environment.NewLine}");
        }
    }
}

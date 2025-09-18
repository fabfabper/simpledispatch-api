using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace SimpleDispatch.Infrastructure
{
    public class MessageProducer<T>
    {
        private readonly string _hostName;
        private readonly string _queueName;

        public MessageProducer(string hostName, string queueName)
        {
            _hostName = hostName;
            _queueName = queueName;
        }

        public void Publish(T message)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        }
    }
}

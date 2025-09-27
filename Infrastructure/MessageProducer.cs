using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;

namespace SimpleDispatch.Infrastructure
{
    public class MessageProducer<T>
    {
        private readonly string _hostName;
        private readonly string _queueName;
        private readonly string? _username;
        private readonly string? _password;

        public MessageProducer(string hostName, string queueName, string? username = null, string? password = null)
        {
            _hostName = hostName;
            _queueName = queueName;
            _username = username;
            _password = password;
        }

        public void Publish(T message)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            if (!string.IsNullOrEmpty(_username)) factory.UserName = _username;
            if (!string.IsNullOrEmpty(_password)) factory.Password = _password;
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        }
    }
}

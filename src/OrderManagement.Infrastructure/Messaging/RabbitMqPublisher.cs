using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Infrastructure.Messaging;

public class RabbitMqPublisher : IPublisherMessageBus
{
    public async Task PublishAsync<T>(string queueName, T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        await using var connection =
            await factory.CreateConnectionAsync();

        await using var channel =
            await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);
    }
}
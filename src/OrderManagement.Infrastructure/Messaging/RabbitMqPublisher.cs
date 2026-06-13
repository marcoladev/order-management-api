using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Infrastructure.Messaging;

public class RabbitMqPublisher : IPublisherMessageBus
{
    private readonly IConnection _connection;

    public RabbitMqPublisher(IConnection connection)
    {
        _connection = connection;
    }

    public async Task PublishAsync<T>(string queueName, T message)
    {
        
        await using var channel = await _connection.CreateChannelAsync();

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
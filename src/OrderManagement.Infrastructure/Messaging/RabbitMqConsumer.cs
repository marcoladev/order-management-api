using System.Text;
using System.Text.Json;
using OrderManagement.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderManagement.Infrastructure.Messaging;

public class RabbitMqConsumer : IConsumerMessageBus
{
    public async Task<T?> ConsumeAsync<T>(string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection =
            await factory.CreateConnectionAsync();

        var channel =
            await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var tcs = new TaskCompletionSource<T?>();

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var json =
                Encoding.UTF8.GetString(ea.Body.ToArray());

            var message =
                JsonSerializer.Deserialize<T>(json);

            await channel.BasicAckAsync(
                deliveryTag: ea.DeliveryTag,
                multiple: false);

            tcs.TrySetResult(message);
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer);

        return await tcs.Task;
    }
}

namespace OrderManagement.Application.Interfaces;

public interface IConsumerMessageBus
{
    Task<T?> ConsumeAsync<T>(string queueName);
}
namespace OrderManagement.Application.Interfaces;

public interface IPublisherMessageBus
{
    Task PublishAsync<T>(string queueName,T message);
}
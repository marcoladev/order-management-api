namespace OrderManagement.Application.Interfaces;

public interface IPublisherMessageBus
{
    Task PublishByEventAsync<T>(string queueName, T message);
}
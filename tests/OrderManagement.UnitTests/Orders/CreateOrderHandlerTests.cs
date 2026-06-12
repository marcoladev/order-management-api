using Moq;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Orders.CreateOrder;
using OrderManagement.Application.Orders.Events;
using OrderManagement.Domain.Entities;

namespace OrderManagement.UnitTests.Orders;
public class CreateOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly Mock<IPublisherMessageBus> _messageBusMock;
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();
        _messageBusMock = new Mock<IPublisherMessageBus>();

        _handler = new CreateOrderHandler(
            _repositoryMock.Object,
            _messageBusMock.Object);
    }

    [Fact]
    public async Task Should_CreateOrder_SaveOrder_AndPublishEvent()
    {
        // Arrange
        var command = new CreateOrderCommand(
            "John Doe",
            150.50m);

        Order? savedOrder = null;

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .Callback<Order>(o => savedOrder = o)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.NotEqual(Guid.Empty, result);

        _repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Order>()),
            Times.Once);

        _messageBusMock.Verify(
            m => m.PublishAsync(
                "order-created",
                It.IsAny<OrderCreatedEvent>()),
            Times.Once);

        Assert.NotNull(savedOrder);
        Assert.Equal(command.CustomerName, savedOrder!.CustomerName);
        Assert.Equal(command.TotalAmount, savedOrder.TotalAmount);
        Assert.Equal(result, savedOrder.Id);
    }
}
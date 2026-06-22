using Moq;
using OrderManagement.Application.Events;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Orders.CreateOrder;
using OrderManagement.Domain.Entities;

namespace OrderManagement.UnitTests.Orders;
public class CreateOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly Mock<IPublisherMessageBus> _messageBusMock;
    private readonly Mock<IAuditLogRepository> _auditLogRepositoryMock;
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();
        _messageBusMock = new Mock<IPublisherMessageBus>();
        _auditLogRepositoryMock = new Mock<IAuditLogRepository>();

        _handler = new CreateOrderHandler(
            _repositoryMock.Object,
            _messageBusMock.Object,
            _auditLogRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_CreateOrder_SaveOrder_AndPublishEvent()
    {
        // Arrange
        var command = new CreateOrderCommand(
            "John Doe",
            new List<CreateOrderItemCommand>()
            {
                new CreateOrderItemCommand("Product A", 1, 50.00m),
                new CreateOrderItemCommand("Product B", 2, 25.00m)
            });

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
            m => m.PublishByEventAsync("",
                It.IsAny<AuditLogEvent>()),
            Times.Once);

        Assert.NotNull(savedOrder);
        Assert.Equal(command.CustomerName, savedOrder!.CustomerName);
        Assert.Equal(result, savedOrder.Id);
    }
}
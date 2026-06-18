using Moq;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Orders.CancelOrder;
using OrderManagement.Domain.Entities;

namespace OrderManagement.UnitTests.Orders;

public class CancelOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly Mock<IPublisherMessageBus> _messageBusMock;
    private readonly Mock<IAuditLogRepository> _auditLogRepositoryMock;
    private readonly CancelOrderHandler _handler;

    public CancelOrderHandlerTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();
        _messageBusMock = new Mock<IPublisherMessageBus>();
        _auditLogRepositoryMock = new Mock<IAuditLogRepository>();

        _messageBusMock = new Mock<IPublisherMessageBus>();

        _handler = new CancelOrderHandler(_repositoryMock.Object, 
        _messageBusMock.Object, 
        _auditLogRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenOrderNotFound()
    {
        // Arrange
        var command = new CancelOrderCommand(Guid.NewGuid());

        _repositoryMock
            .Setup(r => r.GetByIdAsync(command.OrderId))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Order not found.", result.Message);

        _repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Order>()),
            Times.Never);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenOrderAlreadyCancelled()
    {
        // Arrange
        var order = new Order(
            "John Doe",
            new List<OrderItem>(){
                new OrderItem("Product A", 1, 50.00m)
            });

        order.Cancel();

        var command = new CancelOrderCommand(order.Id);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(order.Id))
            .ReturnsAsync(order);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Order is already cancelled.", result.Message);

        _repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Order>()),
            Times.Never);
    }

    [Fact]
    public async Task Should_CancelOrder_WhenOrderExists()
    {
        // Arrange
        var order = new Order(
            "John Doe",
            new List<OrderItem>(){
                new OrderItem("Product A", 1, 50.00m)
            });

        var command = new CancelOrderCommand(order.Id);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(order.Id))
            .ReturnsAsync(order);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Order cancelled successfully.", result.Message);

        Assert.Equal(OrderStatus.Cancelled, order.Status);

        _repositoryMock.Verify(
            r => r.UpdateAsync(order),
            Times.Once);
    }
}
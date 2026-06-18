using Moq;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Orders.RetrieveOrders;
using OrderManagement.Domain.Entities;

namespace OrderManagement.UnitTests.Orders;

public class RetrieveOrdersHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly RetrieveOrdersHandler _handler;

    public RetrieveOrdersHandlerTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();
        _handler = new RetrieveOrdersHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Should_ReturnMappedOrders()
    {
        var orders = new List<Order>
        {
            new("John Doe", new List<OrderItem>(){
                new OrderItem("Product A", 1, 50.00m)
            }),
            new("Jane Smith", new List<OrderItem>(){
                new OrderItem("Product A", 1, 50.00m)
            })
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(orders);

        var result = await _handler.HandleAsync(
            new RetrieveOrdersQuery());

        Assert.Equal(2, result.Orders.Count);

        Assert.Equal(orders[0].Id, result.Orders[0].Id);
        Assert.Equal("John Doe", result.Orders[0].CustomerName);

        Assert.Equal(orders[1].Id, result.Orders[1].Id);
        Assert.Equal("Jane Smith", result.Orders[1].CustomerName);

        _repositoryMock.Verify(
            r => r.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Should_ReturnEmptyList_WhenNoOrdersExist()
    {
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Order>());

        var result = await _handler.HandleAsync(
            new RetrieveOrdersQuery());

        Assert.Empty(result.Orders);

        _repositoryMock.Verify(
            r => r.GetAllAsync(),
            Times.Once);
    }
}
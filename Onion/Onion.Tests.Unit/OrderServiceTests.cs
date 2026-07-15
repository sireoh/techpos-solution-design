using Moq;
using Onion.Application.Services;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;

namespace Onion.Tests.Unit
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task CreateOrderAsync_ShouldCallRepositoryAndReturnOrderId()
        {
            // Arrange
            var mockRepo = new Mock<IOrderRepository>();
            var service = new OrderService(mockRepo.Object);

            // Act
            var result = await service.CreateOrderAsync("Alice", 100.00m);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            mockRepo.Verify(r => r.AddAsync(It.Is<Order>(o =>
                o.CustomerName == "Alice" &&
                o.TotalAmount == 100.00m)), Times.Once);
        }
    }
}

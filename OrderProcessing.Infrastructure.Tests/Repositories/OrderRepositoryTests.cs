using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Repositories;
using OrderProcessing.Domain.Aggregates;
using OrderProcessing.Domain.ValueObjects;
using OrderProcessing.Infrastructure.Repositories;

namespace OrderProcessing.Infrastructure.Tests.Repositories;

public class OrderRepositoryTests
    {
        private readonly OrderProcessingDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;

        public OrderRepositoryTests()
        {
            // Set up an in-memory database for testing purposes
            var options = new DbContextOptionsBuilder<OrderProcessingDbContext>()
                .UseInMemoryDatabase("OrderProcessingTestDb")
                .Options;
            
            _dbContext = new OrderProcessingDbContext(options);
            _orderRepository = new OrderRepository(_dbContext);

            // Clear any existing data
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAsync_OrderExists_ReturnsOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order(orderId, "123 Street", "test@example.com", "encrypted-credit-card");
            order.AddItem(new OrderItem(Guid.NewGuid(), "Product A", 2, 19.99m));

            // Save the order to the in-memory database
            await _orderRepository.SaveAsync(order, CancellationToken.None);

            // Act
            var result = await _orderRepository.GetAsync(orderId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result?.Id);
            Assert.Equal(order.InvoiceAddress, result?.InvoiceAddress);
            Assert.Equal(order.InvoiceEmail, result?.InvoiceEmail);
            Assert.Equal(order.Items.Count, result?.Items.Count);
        }

        [Fact]
        public async Task GetAsync_OrderDoesNotExist_ReturnsNull()
        {
            // Arrange
            var nonExistentOrderId = Guid.NewGuid();

            // Act
            var result = await _orderRepository.GetAsync(nonExistentOrderId, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SaveAsync_OrderIsSaved()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order(orderId, "123 Street", "test@example.com", "encrypted-credit-card");
            order.AddItem(new OrderItem(Guid.NewGuid(), "Product A", 2, 19.99m));

            // Act
            await _orderRepository.SaveAsync(order, CancellationToken.None);

            // Assert
            var savedOrder = await _dbContext.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == orderId);

            Assert.NotNull(savedOrder);
            Assert.Equal(orderId, savedOrder?.Id);
            Assert.Equal(order.InvoiceAddress, savedOrder?.InvoiceAddress);
            Assert.Equal(order.InvoiceEmail, savedOrder?.InvoiceEmail);
            Assert.Equal(order.Items.Count, savedOrder?.OrderItems.Count);
        }
    }
using Microsoft.Extensions.Logging;
using Moq;
using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Exceptions.ItemExceptions;
using ReactApp1.Server.Exceptions.OrderExceptions;
using ReactApp1.Server.Exceptions.StorageExceptions;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;

namespace ReactApp1.Server.UnitTest
{
    public class TestOrderService
    {

        public class TestState
        {
            public FullOrderModel FullOrder;
            public StorageModel Storage;
            public readonly IOrderService OrderService;
            
            public readonly Mock<IOrderRepository> OrderRepository;
            public readonly Mock<IItemRepository> ItemRepository;
            public readonly Mock<IFullOrderRepository> FullOrderRepository;
            public readonly Mock<IEmployeeRepository> EmployeeRepository;
            public readonly Mock<IPaymentRepository> PaymentRepository;
            public readonly Mock<IGiftCardRepository> GiftCardRepository;
            public readonly Mock<IPaymentService> PaymentService;
            public readonly Mock<ILogger<OrderService>> Logger;
            
            public TestState()
            {
                OrderRepository = new Mock<IOrderRepository>();
                ItemRepository = new Mock<IItemRepository>();
                FullOrderRepository = new Mock<IFullOrderRepository>();
                EmployeeRepository = new Mock<IEmployeeRepository>();
                PaymentRepository = new Mock<IPaymentRepository>();
                GiftCardRepository = new Mock<IGiftCardRepository>();
                PaymentService = new Mock<IPaymentService>();
                Logger = new Mock<ILogger<OrderService>>();

                OrderService = new OrderService(OrderRepository.Object, ItemRepository.Object,
                    FullOrderRepository.Object, EmployeeRepository.Object, Logger.Object, PaymentRepository.Object,
                    GiftCardRepository.Object, PaymentService.Object);
            }
        }

        public static TestState BuildTestState()
        {
            var output = new TestState();
            
            // Set up default input values for service methods
            output.FullOrder = new FullOrderModel(1, 101, 2);
            output.Storage = new StorageModel(1, 2, 101, 1000);
            
            
            // Set up default return values for mocked repository methods
            // Change these values as needed in your tests to simulate different scenarios
            output.OrderRepository
                .Setup(x => x.GetOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new OrderModel
                {
                    OrderId = 1,
                    Status = (int)OrderStatusEnum.Open,
                    CreatedByEmployeeId = 2,
                    ReceiveTime = DateTime.MinValue,
                    Refunded = false
                });
            
            output.ItemRepository
                .Setup(x => x.GetItemStorageAsync(It.IsAny<int>()))
                .ReturnsAsync(() => new StorageModel
                {
                    StorageId = 1,
                    EstablishmentId = 2,
                    ItemId = 3,
                    Count = 4
                });
            
            output.ItemRepository
                .Setup(x => x.AddStorageAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            
            output.FullOrderRepository
                .Setup(x => x.GetFullOrderAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => new FullOrderModel
                {
                    OrderId = 1,
                    ItemId = 101,
                    Count = 5,
                });
            
            output.FullOrderRepository
                .Setup(x => x.UpdateItemInOrderCountAsync(It.IsAny<FullOrderModel>()))
                .Returns(Task.CompletedTask);
            
            output.FullOrderRepository
                .Setup(x => x.AddItemToOrderAsync(It.IsAny<FullOrderModel>()))
                .Returns(Task.CompletedTask);
            
            return output;
        }
        
        
        [Fact]
        public async Task AddItemToOrder_OrderStatusIsOpenAndItemIsAvailableInStorage_AddsItemToOrder()
        {
            // arrange
            var state = BuildTestState();
            
            // Simulate a case where the item to add is not yet in order
            state.FullOrderRepository
                .Setup(x => x.GetFullOrderAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // act
            await state.OrderService.AddItemToOrder(state.FullOrder);

            // assert
            state.FullOrderRepository.Verify(
                repo => repo.AddItemToOrderAsync(It.IsAny<FullOrderModel>()),
                Times.Once);
        }
        
        [Fact]
        public async Task AddItemToOrder_OrderStatusIsOpenAndItemIsAvailableInStorage_UpdatesExistingItemInOrderCount()
        {
            // arrange
            var state = BuildTestState();
            
            // Simulate a case where the item to add is already in order
            state.FullOrderRepository
                .Setup(x => x.GetFullOrderAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => state.FullOrder);

            // act
            await state.OrderService.AddItemToOrder(state.FullOrder);

            // assert
            state.FullOrderRepository.Verify(
                repo => repo.UpdateItemInOrderCountAsync(It.IsAny<FullOrderModel>()),
                Times.Once);
        }
        
        [Fact]
        public async Task AddItemToOrder_OrderDoesNotExist_ThrowsOrderNotFoundExceptionException()
        {
            // arrange
            var state = BuildTestState();
            
            state.OrderRepository
                .Setup(x => x.GetOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // act
            Task Act() => state.OrderService.AddItemToOrder(state.FullOrder);

            // assert
            var exception = await Assert.ThrowsAsync<OrderNotFoundException>(Act);
            Assert.Equal($"Order (id = {state.FullOrder.OrderId}) was not found", exception.Message);
        }
        
        [Fact]
        public async Task AddItemToOrder_OrderStatusIsNotOpen_ThrowsOrderStatusConflictException()
        {
            // arrange
            var state = BuildTestState();
            
            var order = new OrderModel
            {
                OrderId = 1,
                Status = (int)OrderStatusEnum.Closed,
                CreatedByEmployeeId = 2,
                ReceiveTime = DateTime.MinValue,
                Refunded = false
            };
            
            state.OrderRepository
                .Setup(x => x.GetOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => order);

            // act
            Task Act() => state.OrderService.AddItemToOrder(state.FullOrder);

            // assert
            var exception = await Assert.ThrowsAsync<OrderStatusConflictException>(Act);
            Assert.Equal($"The operation cannot be performed because the order status is '{(int)OrderStatusEnum.Closed}'", exception.Message);
        }
        
        [Fact]
        public async Task AddItemToOrder_ItemDoesNotExistInStorage_ThrowsItemNotFoundException()
        {
            // arrange
            var state = BuildTestState();
            
            state.ItemRepository
                .Setup(x => x.GetItemStorageAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // act
            Task Act() => state.OrderService.AddItemToOrder(state.FullOrder);

            // assert
            var exception = await Assert.ThrowsAsync<ItemNotFoundException>(Act);
            Assert.Equal($"Item (id = {state.FullOrder.ItemId}) was not found", exception.Message);
        }
        
        [Fact]
        public async Task AddItemToOrder_RequestedItemQuantityExceedsStock_ThrowsStockExhaustedException()
        {
            // arrange
            var state = BuildTestState();
            state.FullOrder = new FullOrderModel(1, 101, 1001);
            state.Storage = new StorageModel(1, 2, 101, 1000);
            
            state.ItemRepository
                .Setup(x => x.GetItemStorageAsync(It.IsAny<int>()))
                .ReturnsAsync(() => state.Storage);

            // act
            Task Act() => state.OrderService.AddItemToOrder(state.FullOrder);

            // assert
            var exception = await Assert.ThrowsAsync<StockExhaustedException>(Act);
            Assert.Equal($"The requested quantity of item (id = {state.FullOrder.ItemId}) exceeds the " +
                         $"available stock. Only {state.Storage.Count} items are in stock", exception.Message);
        }
    }
}


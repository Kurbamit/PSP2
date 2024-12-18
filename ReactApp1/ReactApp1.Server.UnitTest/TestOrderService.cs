using System.Security.Principal;
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
            public readonly Mock<IServiceRepository> ServiceRepository;
            public readonly Mock<IFullOrderServiceRepository> FullOrderServiceRepository;
            public readonly Mock<ILogger<OrderService>> Logger;
            public readonly Mock<IDiscountRepository> DiscountRepository;
            public readonly Mock<ITaxService> TaxService;
            public readonly Mock<IFullOrderTaxRepository> FullOrderTaxRepository;
            public readonly Mock<IFullOrderServiceTaxRepository> FullOrderServiceTaxRepository;
            public readonly Mock<IPrincipal> Principal;
            
            
            public TestState()
            {
                OrderRepository = new Mock<IOrderRepository>();
                ItemRepository = new Mock<IItemRepository>();
                FullOrderRepository = new Mock<IFullOrderRepository>();
                EmployeeRepository = new Mock<IEmployeeRepository>();
                PaymentRepository = new Mock<IPaymentRepository>();
                GiftCardRepository = new Mock<IGiftCardRepository>();
                PaymentService = new Mock<IPaymentService>();
                ServiceRepository = new Mock<IServiceRepository>();
                FullOrderServiceRepository = new Mock<IFullOrderServiceRepository>();
                Logger = new Mock<ILogger<OrderService>>();
                DiscountRepository = new Mock<IDiscountRepository>();
                TaxService = new Mock<ITaxService>();
                FullOrderTaxRepository = new Mock<IFullOrderTaxRepository>();
                FullOrderServiceTaxRepository = new Mock<IFullOrderServiceTaxRepository>();

                Principal = new Mock<IPrincipal>();
                
                // Mocking IPrincipal behavior
                var mockIdentity = new Mock<IIdentity>();
                mockIdentity.Setup(i => i.Name).Returns("TestUser");
                mockIdentity.Setup(i => i.IsAuthenticated).Returns(true);

                Principal.Setup(p => p.Identity).Returns(mockIdentity.Object);
                Principal.Setup(p => p.IsInRole(It.IsAny<string>())).Returns((string role) => role == "MasterAdmin");

                OrderService = new OrderService(OrderRepository.Object, ItemRepository.Object, ServiceRepository.Object,
                    FullOrderRepository.Object, FullOrderServiceRepository.Object, EmployeeRepository.Object, Logger.Object, PaymentRepository.Object,
                    GiftCardRepository.Object, PaymentService.Object, DiscountRepository.Object, TaxService.Object, FullOrderTaxRepository.Object,
                    FullOrderServiceTaxRepository.Object);
            }
        }

        public static TestState BuildTestState()
        {
            var output = new TestState();
            
            // Set up default input values for service methods
            output.FullOrder = new FullOrderModel(1, 1, 101, 2, null);
            output.Storage = new StorageModel(1, 2, 101, 1000);
            
            
            // Set up default return values for mocked repository methods
            // Change these values as needed in your tests to simulate different scenarios
            output.OrderRepository
                .Setup(x => x.GetOrderByIdAsync(It.IsAny<int>(), It.IsAny<IPrincipal>()))
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
                .Setup(x => x.AddItemToOrderAsync(It.IsAny<FullOrderModel>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            output.TaxService
                .Setup(x => x.GetItemTaxes(It.IsAny<int>()))
                .ReturnsAsync(new List<TaxModel>());

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
            await state.OrderService.AddItemToOrder(state.FullOrder, 1, It.IsAny<IPrincipal>());

            // assert
            state.FullOrderRepository.Verify(
                repo => repo.AddItemToOrderAsync(It.IsAny<FullOrderModel>(), It.IsAny<int>()),
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
            await state.OrderService.AddItemToOrder(state.FullOrder, 1, It.IsAny<IPrincipal>());

            // assert
            state.FullOrderRepository.Verify(
                repo => repo.UpdateItemInOrderCountAsync(It.IsAny<FullOrderModel>()),
                Times.Once);
        }
        
        [Fact]
        public async Task AddItemToOrder_OrderDoesNotExist_ThrowsOrderNotFoundException()
        {
            // arrange
            var state = BuildTestState();
            
            state.OrderRepository
                .Setup(x => x.GetOrderByIdAsync(It.IsAny<int>(), It.IsAny<IPrincipal>()))
                .ReturnsAsync(() => null);

            // act
            Task Act() => state.OrderService.AddItemToOrder(state.FullOrder, 1, It.IsAny<IPrincipal>());

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
                .Setup(x => x.GetOrderByIdAsync(It.IsAny<int>(), It.IsAny<IPrincipal>()))
                .ReturnsAsync(() => order);

            // act
            Task Act() => state.OrderService.AddItemToOrder(state.FullOrder, 1, It.IsAny<IPrincipal>());

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
            Task Act() => state.OrderService.AddItemToOrder(state.FullOrder, 1, It.IsAny<IPrincipal>());

            // assert
            var exception = await Assert.ThrowsAsync<ItemNotFoundException>(Act);
            Assert.Equal($"Item (id = {state.FullOrder.ItemId}) was not found", exception.Message);
        }
        
        [Fact]
        public async Task AddItemToOrder_RequestedItemQuantityExceedsStock_ThrowsStockExhaustedException()
        {
            // arrange
            var state = BuildTestState();
            state.FullOrder = new FullOrderModel(1, 1, 101, 1001, null);
            state.Storage = new StorageModel(1, 2, 101, 1000);
            
            state.ItemRepository
                .Setup(x => x.GetItemStorageAsync(It.IsAny<int>()))
                .ReturnsAsync(() => state.Storage);

            // act
            Task Act() => state.OrderService.AddItemToOrder(state.FullOrder, 1, It.IsAny<IPrincipal>());

            // assert
            var exception = await Assert.ThrowsAsync<StockExhaustedException>(Act);
            Assert.Equal($"The requested quantity of item (id = {state.FullOrder.ItemId}) exceeds the " +
                         $"available stock. Only {state.Storage.Count} items are in stock", exception.Message);
        }
        
        [Fact]
        public async Task RemoveItemFromOrder_OrderExistsStatusIsOpenItemCountReduced_RemovesItemFromOrder()
        {
            // arrange
            var state = BuildTestState();
            
            state.FullOrder = new FullOrderModel(1, 1, 101, 2, null);
            
            // This order has 5 units of item 101, and we are removing 2
            // The quantity is updated to 3, but the item stays in the order since it still has units left
            state.FullOrderRepository
                .Setup(x => x.GetFullOrderAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => new FullOrderModel
                {
                    OrderId = 1,
                    ItemId = 101,
                    Count = 5,
                });

            // act
            await state.OrderService.RemoveItemFromOrder(state.FullOrder, It.IsAny<IPrincipal>());

            // assert
            state.FullOrderRepository.Verify(
                repo => repo.UpdateItemInOrderCountAsync(It.IsAny<FullOrderModel>()),
                Times.Once);
        }
        
        [Fact]
        public async Task RemoveItemFromOrder_OrderExistsStatusIsOpenItemCountReducedToZero_RemovesItemFromOrder()
        {
            // arrange
            var state = BuildTestState();
            
            state.FullOrder = new FullOrderModel(1, 1, 101, 5, null);
            
            // This order has 5 units of item 101, and we are removing 5
            // The item is removed from the order because it no longer has any units left
            state.FullOrderRepository
                .Setup(x => x.GetFullOrderAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => new FullOrderModel
                {
                    OrderId = 1,
                    ItemId = 101,
                    Count = 5,
                });

            // act
            await state.OrderService.RemoveItemFromOrder(state.FullOrder, It.IsAny<IPrincipal>());

            // assert
            state.FullOrderRepository.Verify(
                repo => repo.DeleteItemFromOrderAsync(It.IsAny<FullOrderModel>()),
                Times.Once);
        }
        
        [Fact]
        public async Task RemoveItemFromOrder_OrderDoesNotExist_ThrowsOrderNotFoundException()
        {
            // arrange
            var state = BuildTestState();
            
            state.OrderRepository
                .Setup(x => x.GetOrderByIdAsync(It.IsAny<int>(), It.IsAny<IPrincipal>()))
                .ReturnsAsync(() => null);

            // act
            Task Act() => state.OrderService.RemoveItemFromOrder(state.FullOrder, It.IsAny<IPrincipal>());

            // assert
            var exception = await Assert.ThrowsAsync<OrderNotFoundException>(Act);
            Assert.Equal($"Order (id = {state.FullOrder.OrderId}) was not found", exception.Message);
        }
        
        [Fact]
        public async Task RemoveItemFromOrder_OrderStatusIsNotOpen_ThrowsOrderStatusConflictException()
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
                .Setup(x => x.GetOrderByIdAsync(It.IsAny<int>(), It.IsAny<IPrincipal>()))
                .ReturnsAsync(() => order);

            // act
            Task Act() => state.OrderService.RemoveItemFromOrder(state.FullOrder, It.IsAny<IPrincipal>());

            // assert
            var exception = await Assert.ThrowsAsync<OrderStatusConflictException>(Act);
            Assert.Equal($"The operation cannot be performed because the order status is '{(int)OrderStatusEnum.Closed}'", exception.Message);
        }
        
        [Fact]
        public async Task RemoveItemFromOrder_RemovedItemCountExceedsItemInOrderQuantity_RemovesItemFromOrder()
        {
            // arrange
            var state = BuildTestState();
            
            state.FullOrder = new FullOrderModel(1, 1, 101, 10, null);
            
            state.FullOrderRepository
                .Setup(x => x.GetFullOrderAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => new FullOrderModel
                {
                    OrderId = 1,
                    ItemId = 101,
                    Count = 5,
                });

            // act
            await state.OrderService.RemoveItemFromOrder(state.FullOrder, It.IsAny<IPrincipal>());

            // assert
            state.FullOrderRepository.Verify(
                repo => repo.DeleteItemFromOrderAsync(It.IsAny<FullOrderModel>()),
                Times.Once);
        }
        
        [Fact]
        public async Task RemoveItemFromOrder_ItemNotFoundInOrder_RemovesItemFromOrder()
        {
            // arrange
            var state = BuildTestState();
            
            state.FullOrder = new FullOrderModel(1, 1, 101, 10, null);
            
            state.FullOrderRepository
                .Setup(x => x.GetFullOrderAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);
            
            // act
            Task Act() => state.OrderService.RemoveItemFromOrder(state.FullOrder, It.IsAny<IPrincipal>());

            // assert
            var exception = await Assert.ThrowsAsync<ItemNotFoundInOrderException>(Act);
            Assert.Equal($"Item (id = {state.FullOrder.ItemId}) was not found in order (id = {state.FullOrder.OrderId})", exception.Message);
        }
        
        [Fact]
        public async Task OpenOrder_UserHasValidAccessToken_OpensOrder()
        {
            // arrange
            var state = BuildTestState();
            
            state.OrderRepository
                .Setup(x => x.AddEmptyOrderAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => new OrderModel
                {
                    OrderId = 1,
                    CreatedByEmployeeId = 2,
                    Status = (int)OrderStatusEnum.Open
                });
            
            // act
            var order = await state.OrderService.OpenOrder(2, 3);
            
            // assert
            Assert.Equal(1, order.Order.OrderId);
            Assert.Equal(2, order.Order.CreatedByEmployeeId);
            Assert.Equal(1, order.Order.Status);
        }
        
        [Fact]
        public async Task OpenOrder_UserDoesNotHaveValidAccessToken_ThrowsUnauthorizedAccessException()
        {
            // arrange
            var state = BuildTestState();
            
            state.OrderRepository
                .Setup(x => x.AddEmptyOrderAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => new OrderModel
                {
                    OrderId = 1,
                    CreatedByEmployeeId = 2,
                    Status = (int)OrderStatusEnum.Open
                });
            
            // act
            Task Act() => state.OrderService.OpenOrder(null, null);
            
            // assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(Act);
            Assert.Equal($"Operation failed: Invalid or expired access token", exception.Message);
        }
        
    }
}


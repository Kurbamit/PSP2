using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IFullOrderRepository _fullOrderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IItemRepository itemRepository, 
            IFullOrderRepository fullOrderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _fullOrderRepository = fullOrderRepository;
            _logger = logger;
        }
        
        public async Task<OrderItems> OpenOrder(int? createdByEmployeeId)
        {
            if (!createdByEmployeeId.HasValue)
                throw new ArgumentNullException(nameof(createdByEmployeeId));
            
            var emptyOrder = await _orderRepository.AddEmptyOrderAsync(createdByEmployeeId.Value);

            return new OrderItems(emptyOrder, null);
        }
        
        public Task<PaginatedResult<OrderModel>> GetAllOrders(int pageNumber, int pageSize)
        {
            return _orderRepository.GetAllOrdersAsync(pageNumber, pageSize);
        }

        public async Task<OrderItems> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogInformation($"Order with id: {orderId} not found");
                return new OrderItems(null, null);
            }

            var orderItems = await GetOrderItems(orderId);
            return new OrderItems(order, orderItems);
        }
        
        private async Task<List<ItemModel>> GetOrderItems(int orderId)
        {
            var orderItemIds = await _fullOrderRepository.GetOrderItemsAsync(orderId);
            
            var orderItems = new List<ItemModel>();
            foreach (var id in orderItemIds)
            {
                var item = await _itemRepository.GetItemByIdAsync(id);
                if (item != null)
                    orderItems.Add(item);
            }

            return orderItems;
        }
        
        public async Task AddItemToOrder(FullOrderModel fullOrder)
        {
            // Before adding an item to an order, check if:
            // 1. The order exists
            // 2. The item exists and there is enough stock in storage
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIsOpen(fullOrder.OrderId, "AddItemToOrder");
            if (existingOrderWithOpenStatus == null && await ItemIsAvailableInStorage())
                return;
            
            var existingFullOrder = await _fullOrderRepository.GetFullOrderAsync(fullOrder.OrderId, fullOrder.ItemId);
            
            // TODO: Remove the reserved quantity of items from storage
            
            var task = existingFullOrder != null
                // If the item is already in the order (fullOrder record which links the order with the item exists in the database)
                // update its quantity by adding new count to existing count
                ? _fullOrderRepository.UpdateItemInOrderCountAsync(fullOrder)
                // Otherwise, create a new record for it
                : _fullOrderRepository.AddItemToOrderAsync(fullOrder);
            
            await task;

            async Task<bool> ItemIsAvailableInStorage()
            {
                var storage = await _itemRepository.GetItemStorageAsync(fullOrder.ItemId);
                if (storage == null)
                {
                    _logger.LogInformation($"Failed to add item {fullOrder.ItemId} to order {fullOrder.OrderId}: Item not found in storage");
                    return false;
                }
                
                if (storage.Count < fullOrder.Count)
                {
                    _logger.LogInformation($"Not enough stock for item {fullOrder.ItemId}. Requested: {fullOrder.Count}, Available: {storage.Count} for order {fullOrder.OrderId}");
                    return false;
                }

                return true;
            }
        }
        
        public async Task RemoveItemFromOrder(FullOrderModel fullOrder)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIsOpen(fullOrder.OrderId, "RemoveItemFromOrder");
            if (existingOrderWithOpenStatus == null)
                return;
            
            var existingFullOrder = await _fullOrderRepository.GetFullOrderAsync(fullOrder.OrderId, fullOrder.ItemId);
            if (existingFullOrder == null)
            {
                _logger.LogInformation($"The specified item {fullOrder.ItemId} is not linked to the given order {fullOrder.OrderId}");
                return;
            }
            
            // TODO: Add reserved quantity of items back to storage
            
            await _fullOrderRepository.DeleteItemFromOrderAsync(existingFullOrder);
        }
        
        public async Task UpdateOrder(OrderModel order)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIsOpen(order.OrderId, "UpdateOrder");
            if (existingOrderWithOpenStatus == null)
                return;
            
            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task CloseOrder(int orderId)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIsOpen(orderId, "CloseOrder");
            if(existingOrderWithOpenStatus == null)
                return;

            existingOrderWithOpenStatus.Status = (int)OrderStatusEnum.Closed;
            
            await _orderRepository.UpdateOrderAsync(existingOrderWithOpenStatus);
        }
        
        private async Task<OrderModel?> GetOrderIfExistsAndStatusIsOpen(int orderId, string? operation = null)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogInformation($"Operation '{operation}' failed: Order {orderId} not found");
                return null;
            }

            if (order.Status != (int)OrderStatusEnum.Open)
            {
                _logger.LogInformation($"Operation '{operation}' failed: Order status is {order.Status.ToString()}");
                return null;
            }
                
            return order;
        }
    }
}


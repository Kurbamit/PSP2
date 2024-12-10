using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Exceptions.ItemExceptions;
using ReactApp1.Server.Exceptions.OrderExceptions;
using ReactApp1.Server.Exceptions.StorageExceptions;
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
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IItemRepository itemRepository, 
            IFullOrderRepository fullOrderRepository, IEmployeeRepository employeeRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _fullOrderRepository = fullOrderRepository;
            _employeeRepository = employeeRepository;
            _logger = logger;
        }
        
        public async Task<OrderItems> OpenOrder(int? createdByEmployeeId, int? establishmentId)
        {
            if (!createdByEmployeeId.HasValue || !establishmentId.HasValue)
            {
                _logger.LogError("Failed to open order: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }
            
            var emptyOrder = await _orderRepository.AddEmptyOrderAsync(createdByEmployeeId.Value, establishmentId.Value);

            return new OrderItems(emptyOrder, null);
        }
        
        public async Task<PaginatedResult<OrderModel>> GetAllOrders(int pageNumber, int pageSize)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize);
            foreach (var order in orders.Items)
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(order.CreatedByEmployeeId);
                if(employee != null)
                    order.CreatedByEmployeeName = employee.FirstName + " " + employee.LastName;
            }
            
            return orders;
        }

        public async Task<OrderItems> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogInformation($"Order with id: {orderId} not found");
                return new OrderItems(null, null);
            }
            
            var employee = await _employeeRepository.GetEmployeeByIdAsync(order.CreatedByEmployeeId);
            if(employee != null)
                order.CreatedByEmployeeName = employee.FirstName + " " + employee.LastName;

            var orderItems = await GetOrderItems(orderId);

            var orderWithTotalPrice = CalculateTotalPriceForOrder(order, orderItems);
            
            return new OrderItems(orderWithTotalPrice, orderItems);
        }
        
        private async Task<List<ItemModel>> GetOrderItems(int orderId)
        {
            var fullOrders = await _fullOrderRepository.GetOrderItemsAsync(orderId);
            
            var orderItems = new List<ItemModel>();
            foreach (var fullOrder in fullOrders)
            {
                var item = await _itemRepository.GetItemByIdAsync(fullOrder.ItemId);
                
                if(item == null)
                    continue;
                
                item.Count = fullOrder.Count;
                orderItems.Add(item);
            }

            return orderItems;
        }

        private OrderModel CalculateTotalPriceForOrder(OrderModel order, List<ItemModel> orderItems)
        {
            decimal totalPrice = 0;
            foreach (var item in orderItems)
            {
                decimal itemCost = item.Cost ?? 0;
                decimal itemCount = item.Count ?? 0;

                
                totalPrice += itemCost * itemCount;
            }
            
            // TODO: Apply discount and taxes to the total price
            
            order.TotalPrice = totalPrice;

            return order;
        }
        
        public async Task AddItemToOrder(FullOrderModel fullOrder)
        {
            // Before adding an item to an order, check if:
            // 1. The order exists
            // 2. The item exists and there is enough stock in storage
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIsOpen(fullOrder.OrderId, "AddItemToOrder");
            if (existingOrderWithOpenStatus == null) 
                return;

            var itemIsAvailableInStorage = await ItemIsAvailableInStorage();
            if (!itemIsAvailableInStorage)
                return;
            
            var existingFullOrder = await _fullOrderRepository.GetFullOrderAsync(fullOrder.OrderId, fullOrder.ItemId);
            
            // Reduce reserved item count in storage
            await _itemRepository.AddStorageAsync(fullOrder.ItemId, -fullOrder.Count);
            
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
                    _logger.LogError($"Failed to add item {fullOrder.ItemId} to order {fullOrder.OrderId}: Item not found in storage");
                    throw new ItemNotFoundException(fullOrder.ItemId);
                }
                
                if (storage.Count < fullOrder.Count)
                {
                    _logger.LogError($"Not enough stock for item {fullOrder.ItemId}. Requested: {fullOrder.Count}, Available: {storage.Count} for order {fullOrder.OrderId}");
                    throw new StockExhaustedException(fullOrder.ItemId, storage.Count);
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
                _logger.LogError($"The specified item {fullOrder.ItemId} is not linked to the given order {fullOrder.OrderId}");
                throw new ItemNotFoundInOrderException(fullOrder.ItemId, fullOrder.OrderId);
            }
            
            // Ensure we don't return more items to storage than were originally reserved, this check handles that
            var itemsToRemoveCount =  Math.Min(fullOrder.Count, existingFullOrder.Count);
            
            await _itemRepository.AddStorageAsync(fullOrder.ItemId, itemsToRemoveCount);
            
            if (fullOrder.Count < existingFullOrder.Count)
            {
                // If the count of item to remove is smaller, update the quantity, but don't remove the item from the order
                fullOrder.Count = -fullOrder.Count;
                _logger.LogInformation($"Updating item count {fullOrder.Count}");
                await _fullOrderRepository.UpdateItemInOrderCountAsync(fullOrder);
                return;

            }
            // Otherwise, delete the item from the order
            await  _fullOrderRepository.DeleteItemFromOrderAsync(fullOrder);
        }
        
        public async Task UpdateOrder(OrderModel order)
        {
            var existingOrderWithOpenStatus = await GetOrderIfExistsAndStatusIsOpen(order.OrderId, "UpdateOrder");
            if (existingOrderWithOpenStatus == null)
                throw new OrderNotFoundException(order.OrderId);
            
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
                _logger.LogError($"Operation '{operation}' failed: Order {orderId} not found");
                throw new OrderNotFoundException(orderId);
            }

            if (order.Status != (int)OrderStatusEnum.Open)
            {
                _logger.LogError($"Operation '{operation}' failed: Order status is {order.Status.ToString()}");
                throw new OrderStatusConflictException(order.Status.ToString());
            }
                
            return order;
        }
    }
}


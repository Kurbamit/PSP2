using ReactApp1.Server.Data.Repositories;
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
        
        public Task<OrderModel> OpenOrder(int? createdByEmployeeId)
        {
            if (!createdByEmployeeId.HasValue)
                throw new ArgumentNullException(nameof(createdByEmployeeId));
            
            return _orderRepository.AddEmptyOrderAsync(createdByEmployeeId.Value);
        }
        
        public Task<PaginatedResult<OrderModel>> GetAllOrders(int pageNumber, int pageSize)
        {
            return _orderRepository.GetAllOrdersAsync(pageNumber, pageSize);
        }

        public Task<OrderModel?> GetOrderById(int orderId)
        {
            return _orderRepository.GetOrderByIdAsync(orderId);
        }
        
        public async Task AddItemToOrder(FullOrderModel fullOrder)
        {
            // Before adding an item to an order, check if:
            // 1. The order exists
            // 2. The item exists and there is enough stock in storage
            if (!(await OrderExists() && await ItemIsAvailableInStorage()))
                return;
            
            var existingFullOrder = await _fullOrderRepository.GetFullOrderByIdAsync(fullOrder.FullOrderId);
            
            // If the item is already in the order (fullOrder record which links the order with the item exists in the database)
            // update its quantity by adding new count to existing count
            if(existingFullOrder != null)
                await _fullOrderRepository.UpdateItemInOrderCountAsync(existingFullOrder);

            // Otherwise, create a new record for it
            await _fullOrderRepository.AddItemToOrderAsync(fullOrder);

            async Task<bool> OrderExists()
            {
                var order = await _orderRepository.GetOrderByIdAsync(fullOrder.OrderId);
                if (order == null)
                {
                    _logger.LogInformation($"Failed to add item {fullOrder.ItemId} to order {fullOrder.ItemId}: Order not found");
                    return false;
                }
                
                return true;
            }

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
        
        public Task RemoveItemFromOrder(int itemId)
        {
            // TODO
            // _fullOrderRepository.DeleteItemFromOrderAsync(itemId);
            return Task.CompletedTask;
        }

        public Task<OrderModel?> AddDiscountToOrderAsync()
        {
            // TODO:
            // return _orderRepository.AddDiscountToOrderAsync();
            return Task.FromResult<OrderModel?>(null);
        }

        public Task CloseOrder(int orderId)
        {
            // TODO
            // _orderRepository.DeleteOrderAsync(orderId);
            return Task.CompletedTask;
        }
    }
}


using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IFullOrderRepository _fullOrderRepository;
    private readonly ILogger<OrderRepository> _logger;

    public OrderService(IOrderRepository orderRepository, IItemRepository itemRepository, 
        IFullOrderRepository fullOrderRepository, Logger<OrderRepository> logger)
    {
        _orderRepository = orderRepository;
        _itemRepository = itemRepository;
        _fullOrderRepository = fullOrderRepository;
        _logger = logger;
    }
    
    public async Task AddItemToOrder(FullOrderModel fullOrder)
    {
        var order = await _orderRepository.GetOrderByIdAsync(fullOrder.OrderId);
        if (order == null)
        {
            _logger.LogWarning($"Order {fullOrder.OrderId} not found when adding item {fullOrder.ItemId}");
            return;
        }
        
        var item = await _itemRepository.GetItemByIdAsync(fullOrder.ItemId);
        if (item == null)
        {
            _logger.LogWarning($"Item {fullOrder.ItemId} not found when adding it to order {fullOrder.OrderId}");
            return;
        }

        await _fullOrderRepository.AddItemToOrderAsync(fullOrder);
    }
    
}
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

public interface IFullOrderRepository
{
    Task AddItemToOrderAsync(FullOrderModel fullOrder);
    Task<FullOrderModel?> GetFullOrderAsync(int orderId, int itemId);
    Task<List<int>> GetOrderItemsAsync(int orderId);
    Task UpdateItemInOrderCountAsync(FullOrderModel fullOrder);
    Task DeleteItemFromOrderAsync(FullOrderModel fullOrder);
}
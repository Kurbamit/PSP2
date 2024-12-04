using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

public interface IFullOrderRepository
{
    Task AddItemToOrderAsync(FullOrderModel fullOrder);
    Task<OrderModel?> DeleteItemFromOrderAsync(int itemId);
}
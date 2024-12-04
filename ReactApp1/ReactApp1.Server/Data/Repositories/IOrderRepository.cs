using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

public interface IOrderRepository
{
    Task<PaginatedResult<OrderModel>> GetAllOrdersAsync(int pageNumber, int pageSize);
    Task<OrderModel?> GetOrderByIdAsync(int orderId);
    Task AddOrderAsync(OrderModel order);
    Task<OrderModel?> AddDiscountToOrderAsync();
    
    Task<OrderModel?> DeleteOrderAsync(int itemId);
}
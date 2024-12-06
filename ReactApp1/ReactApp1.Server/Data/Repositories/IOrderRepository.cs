using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

public interface IOrderRepository
{
    Task<OrderModel> AddEmptyOrderAsync(int createdByEmployeeId);
    Task<PaginatedResult<OrderModel>> GetAllOrdersAsync(int pageNumber, int pageSize);
    Task<OrderModel?> GetOrderByIdAsync(int orderId);
    Task<OrderModel?> AddDiscountToOrderAsync();
    Task DeleteOrderAsync(int orderId);
}
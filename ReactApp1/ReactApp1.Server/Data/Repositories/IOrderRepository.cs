using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

public interface IOrderRepository
{
    Task<OrderModel> AddEmptyOrderAsync(int createdByEmployeeId, int establishmentId);
    Task<PaginatedResult<OrderModel>> GetAllOrdersAsync(int pageNumber, int pageSize);
    Task<OrderModel?> GetOrderByIdAsync(int orderId);
    Task UpdateOrderAsync(OrderModel order);
    Task DeleteOrderAsync(int orderId);
    Task<byte[]> DownloadReceipt(OrderItemsPayments order);
}
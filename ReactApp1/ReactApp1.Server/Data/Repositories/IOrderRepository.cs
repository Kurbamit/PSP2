using System.Security.Principal;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

public interface IOrderRepository
{
    Task<OrderModel> AddEmptyOrderAsync(int createdByEmployeeId, int establishmentId);
    Task<PaginatedResult<OrderModel>> GetAllOrdersAsync(int pageNumber, int pageSize, IPrincipal user);
    Task<OrderModel?> GetOrderByIdAsync(int orderId, IPrincipal user);
    Task UpdateOrderAsync(OrderModel order);
    Task DeleteOrderAsync(int orderId);
    Task<byte[]> DownloadReceipt(OrderItemsPayments order);
}
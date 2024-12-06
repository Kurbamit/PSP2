using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IOrderService
{
    Task<OrderModel> OpenOrder(int? createdByEmployeeId);
    Task<PaginatedResult<OrderModel>> GetAllOrders(int pageNumber, int pageSize);
    Task<OrderModel?> GetOrderById(int orderId);
    Task AddItemToOrder(FullOrderModel fullOrder);
    Task RemoveItemFromOrder(int itemId);
    Task<OrderModel?> AddDiscountToOrderAsync();
    Task CloseOrder(int orderId);
}
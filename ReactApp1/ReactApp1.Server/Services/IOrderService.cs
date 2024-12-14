using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IOrderService
{
    Task<OrderItemsPayments> OpenOrder(int? createdByEmployeeId, int? establishmentId);
    Task<PaginatedResult<OrderModel>> GetAllOrders(int pageNumber, int pageSize);
    Task<OrderItemsPayments> GetOrderById(int orderId);
    Task AddItemToOrder(FullOrderModel fullOrder, int? userId);
    Task UpdateOrder(OrderModel order);
    Task RemoveItemFromOrder(FullOrderModel fullOrder);
    Task CloseOrder(int orderId);
    Task CancelOrder(int orderId);
    Task RefundOrder(int orderId);
    Task TipOrder(TipModel tip);
    Task PayOrder(PaymentModel payment);
    Task<byte[]> DownloadReceipt(int orderId);
}
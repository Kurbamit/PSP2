using System.Security.Principal;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IOrderService
{
    Task<OrderItemsPayments> OpenOrder(int? createdByEmployeeId, int? establishmentId);
    Task<PaginatedResult<OrderModel>> GetAllOrders(int pageNumber, int pageSize, IPrincipal user);
    Task<OrderItemsPayments> GetOrderById(int orderId, IPrincipal user);
    Task AddItemToOrder(FullOrderModel fullOrder, int? userId);
    Task AddServiceToOrder(FullOrderServiceModel fullOrderServiceModel, int? userId);
    Task UpdateOrder(OrderModel order);
    Task RemoveItemFromOrder(FullOrderModel fullOrder);
    Task RemoveServiceFromOrder(FullOrderServiceModel fullOrderService);
    Task CloseOrder(int orderId);
    Task CancelOrder(int orderId);
    Task RefundOrder(int orderId);
    Task TipOrder(TipModel tip);
    Task DiscountOrder(DiscountModel discount);
    Task PayOrder(PaymentModel payment, IPrincipal user);
    Task<byte[]> DownloadReceipt(int orderId, IPrincipal user);
}
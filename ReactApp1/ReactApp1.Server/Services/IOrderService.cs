using System.Security.Principal;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IOrderService
{
    Task<OrderItemsPayments> OpenOrder(int? createdByEmployeeId, int? establishmentId);
    Task<PaginatedResult<OrderModel>> GetAllOrders(int pageNumber, int pageSize, IPrincipal user);
    Task<OrderItemsPayments> GetOrderById(int orderId, IPrincipal user);
    Task AddItemToOrder(FullOrderModel fullOrder, int? userId, IPrincipal user);
    Task AddServiceToOrder(FullOrderServiceModel fullOrderServiceModel, int? userId, IPrincipal user);
    Task UpdateOrder(OrderModel order, IPrincipal user);
    Task RemoveItemFromOrder(FullOrderModel fullOrder, IPrincipal user);
    Task RemoveServiceFromOrder(FullOrderServiceModel fullOrderService, IPrincipal user);
    Task CloseOrder(int orderId, IPrincipal user);
    Task CancelOrder(int orderId, IPrincipal user);
    Task RefundOrder(int orderId, IPrincipal user);
    Task TipOrder(TipModel tip, IPrincipal user);
    Task DiscountOrder(DiscountModel discount, IPrincipal user);
    Task PayOrder(PaymentModel payment, IPrincipal user);
    Task<byte[]> DownloadReceipt(int orderId, IPrincipal user);
}
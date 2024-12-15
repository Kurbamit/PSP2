using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

public interface IFullOrderServiceRepository
{
    Task AddServiceToOrderAsync(FullOrderServiceModel fullOrderService, int userId);
    Task<FullOrderServiceModel?> GetFullOrderServiceAsync(int orderId, int serviceId);
    Task<List<FullOrderServiceModel>> GetOrderServicesAsync(int orderId);
    Task UpdateServiceInOrderCountAsync(FullOrderServiceModel fullOrderService);
    Task DeleteServiceFromOrderAsync(FullOrderServiceModel fullOrderService);
    Task UpdateFullOrderServiceDiscountAsync(FullOrderServiceModel fullOrderService);
}
using System.Security.Principal;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IServiceRepository
    {
        Task<PaginatedResult<Service>> GetAllServicesAsync(int pageNumber, int pageSize, IPrincipal user);
        Task<ServiceModel?> GetServiceByIdAsync(int employeeId, IPrincipal user);
        Task<ServiceModel?> GetServiceByIdFromFullOrderAsync(int serviceId, int orderId);
        Task<Service> AddServiceAsync(ServiceModel service);
        Task UpdateServiceAsync(ServiceModel service);
        Task DeleteServiceAsync(int serviceId);
    }
}
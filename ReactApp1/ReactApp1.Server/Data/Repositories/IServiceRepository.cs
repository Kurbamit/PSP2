using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IServiceRepository
    {
        Task<PaginatedResult<Service>> GetAllServicesAsync(int pageNumber, int pageSize);
        Task<ServiceModel?> GetServiceByIdAsync(int employeeId);
        Task<Service> AddServiceAsync(ServiceModel service);
        Task UpdateServiceAsync(ServiceModel service);
        Task DeleteServiceAsync(int serviceId);
    }
}
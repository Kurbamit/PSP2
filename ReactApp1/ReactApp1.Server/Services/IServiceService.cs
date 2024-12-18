using System.Security.Principal;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IServiceService
{
    Task<PaginatedResult<Service>> GetAllServices(int pageSize, int pageNumber, IPrincipal user);
    Task<ServiceModel?> GetServiceById(int serviceId, IPrincipal user);
    Task<Service> CreateNewService(ServiceModel service, int? establishmentId);
    Task UpdateService(ServiceModel service);
    Task DeleteService(int serviceId);
}
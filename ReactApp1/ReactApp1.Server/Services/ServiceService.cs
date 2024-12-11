using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger<ServiceService> _logger;

        public ServiceService(IServiceRepository serviceRepository, ILogger<ServiceService> logger)
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
        }

        public Task<PaginatedResult<Service>> GetAllServices(int pageSize, int pageNumber)
        {
            return _serviceRepository.GetAllServicesAsync(pageSize, pageNumber);
        }

        public Task<ServiceModel?> GetServiceById(int serviceId)
        {
            return _serviceRepository.GetServiceByIdAsync(serviceId);
        }

        public Task<Service> CreateNewService(ServiceModel service, int? establishmentId)
        {
            if (!establishmentId.HasValue)
            {
                _logger.LogError("Failed to add service: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }

            service.EstablishmentId = establishmentId.Value;

            return _serviceRepository.AddServiceAsync(service);
        }

        public Task UpdateService(ServiceModel service)
        {
            return _serviceRepository.UpdateServiceAsync(service);
        }

        public Task DeleteService(int serviceId)
        {
            return _serviceRepository.DeleteServiceAsync(serviceId);
        }
    }
}
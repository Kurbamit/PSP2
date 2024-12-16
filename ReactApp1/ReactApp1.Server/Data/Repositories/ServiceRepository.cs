using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Exceptions.GiftCardExceptions;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Service>> GetAllServicesAsync(int pageNumber, int pageSize, IPrincipal user)
        {
            var totalItems = await _context.Set<Service>().FilterByAuthorizedUser(user).CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var services = await _context.Set<Service>()
                .FilterByAuthorizedUser(user)
                .OrderBy(service => service.ServiceId)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();

            return new PaginatedResult<Service>
            {
                Items = services,
                TotalPages = totalPages,
                TotalItems = totalItems,
                CurrentPage = pageNumber
            };
        }

        public async Task<ServiceModel?> GetServiceByIdAsync(int serviceId, IPrincipal user)
        {
            var service = await _context.Services
                .FilterByAuthorizedUser(user)
                .Where(f => f.ServiceId == serviceId)
                .Select(f => new ServiceModel()
                {
                    ServiceId = f.ServiceId,
                    Name = f.Name,
                    EstablishmentId = f.EstablishmentId,
                    Cost = f.Cost,
                    Tax = f.Tax,
                    ServiceLength = f.ServiceLength,
                    ReceiveTime = f.ReceiveTime
                }).FirstOrDefaultAsync();

            if (service == null)
            {
                throw new AuthorizationException();
            }

            return service;
        }


        public async Task<ServiceModel?> GetServiceByIdFromFullOrderAsync(int serviceId, int orderId)
        {
            var service = await _context.FullOrderServices
                .Where(f => f.ServiceId == serviceId && f.OrderId == orderId)
                .Select(f => new ServiceModel()
                {
                    ServiceId = f.ServiceId,
                    Name = f.Name,
                    Cost = f.Cost,
                    Tax = f.Tax,
                    ServiceLength = f.ServiceLength,
                    ReceiveTime = f.ReceiveTime
                }).FirstOrDefaultAsync();

            return service;
        }

        public async Task<Service> AddServiceAsync(ServiceModel service)
        {
            try
            {
                var newService = new Service
                {
                    Name = service.Name,
                    EstablishmentId = service.EstablishmentId,
                    ServiceLength = service.ServiceLength,
                    Cost = service.Cost,
                    Tax = service.Tax,
                    ReceiveTime = service.ReceiveTime
                };

                var serviceEntity = await _context.Services.AddAsync(newService);
                await _context.SaveChangesAsync();

                return serviceEntity.Entity;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new service to the database.", e);
            }
        }

        public async Task UpdateServiceAsync(ServiceModel service)
        {
            try
            {
                var existingService = await _context.Set<Service>()
                    .FirstOrDefaultAsync(i => i.ServiceId == service.ServiceId);


                if (existingService == null)
                {
                    throw new KeyNotFoundException($"Service with ID {service.ServiceId} not found.");
                }

                service.MapUpdate(existingService);
                _context.Set<Service>().Update(existingService);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating the service: {service.ServiceId}.", e);
            }
        }

        public async Task DeleteServiceAsync(int serviceId)
        {
            try
            {
                _context.Set<Service>().Remove(new Service
                {
                    ServiceId = serviceId
                });

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the service {serviceId} from the database.", e);
            }
        }
    }
}


using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Exceptions.ItemExceptions;
using ReactApp1.Server.Exceptions.ServiceExceptions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public class FullOrderServiceRepository : IFullOrderServiceRepository
    {

        private readonly AppDbContext _context;
        private readonly ILogger<FullOrderServiceRepository> _logger;

        public FullOrderServiceRepository(AppDbContext context, ILogger<FullOrderServiceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddServiceToOrderAsync(FullOrderServiceModel fullOrder, int userId)
        {
            try
            {
                var newFullOrder = await _context.Services
                    .Where(f => f.ServiceId == fullOrder.ServiceId)
                    .Select(f => new FullOrderService()
                    {
                        OrderId = fullOrder.OrderId,
                        ServiceId = fullOrder.ServiceId,
                        Count = fullOrder.Count,
                        Name = f.Name,
                        Cost = f.Cost,
                        Tax = f.Tax,
                        ServiceLength = f.ServiceLength,
                        ReceiveTime = DateTime.UtcNow,
                        CreatedByEmployeeId = userId
                    }).FirstOrDefaultAsync();

                if (newFullOrder == null)
                {
                    throw new ItemNotFoundException(fullOrder.ServiceId);
                }

                await _context.Set<FullOrderService>().AddAsync(newFullOrder);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException($"An error occurred while adding fullOrderService (orderId ={fullOrder.OrderId}, serviceId = {fullOrder.ServiceId}) record to the database.", e);
            }
        }

        public async Task<FullOrderServiceModel?> GetFullOrderServiceAsync(int orderId, int serviceId)
        {
            return await _context.FullOrderServices
                .Where(f => f.OrderId == orderId && f.ServiceId == serviceId)
                .Select(f => new FullOrderServiceModel(f))
                .FirstOrDefaultAsync();
        }

        public async Task<List<FullOrderServiceModel>> GetOrderServicesAsync(int orderId)
        {
            return await _context.FullOrderServices
                .Where(f => f.OrderId == orderId)
                .Select(f => new FullOrderServiceModel(f))
                .ToListAsync();
        }


        public async Task UpdateServiceInOrderCountAsync(FullOrderServiceModel fullOrder)
        {
            var existingFullOrder = await _context.FullOrderServices
                .Where(f => f.OrderId == fullOrder.OrderId && f.ServiceId == fullOrder.ServiceId)
                .FirstOrDefaultAsync();

            if (existingFullOrder == null)
            {
                _logger.LogError($"Service {fullOrder.ServiceId} was not found in order {fullOrder.OrderId}");
                throw new ServiceNotFoundInOrderException(fullOrder.OrderId, fullOrder.ServiceId);
            }

            // update service's quantity by adding new count to existing count
            existingFullOrder.Count += fullOrder.Count;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceFromOrderAsync(FullOrderServiceModel fullOrder)
        {
            var existingFullOrder = await _context.FullOrderServices
                .Where(f => f.OrderId == fullOrder.OrderId && f.ServiceId == fullOrder.ServiceId)
                .FirstOrDefaultAsync();

            if (existingFullOrder == null)
            {
                _logger.LogError($"Service {fullOrder.ServiceId} was not found in order {fullOrder.OrderId}");
                throw new ServiceNotFoundInOrderException(fullOrder.OrderId, fullOrder.ServiceId);
            }

            try
            {
                _context.FullOrderServices.Remove(existingFullOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException($"An error occurred while deleting fullOrderService (orderId = {fullOrder.OrderId}, serviceId = {fullOrder.ServiceId}) record to the database.", e);
            }
        }

        public async Task UpdateFullOrderServiceDiscountAsync(FullOrderServiceModel fullOrderService)
        {
            var existingFullOrder = await _context.FullOrderServices
                .Where(f => f.OrderId == fullOrderService.OrderId && f.ServiceId == fullOrderService.ServiceId)
                .FirstOrDefaultAsync();

            if (existingFullOrder == null)
            {
                _logger.LogError($"Service {fullOrderService.ServiceId} was not found in order {fullOrderService.OrderId}");
                throw new ServiceNotFoundInOrderException(fullOrderService.OrderId, fullOrderService.ServiceId);
            }

            existingFullOrder.DiscountId = fullOrderService.DiscountId;

            await _context.SaveChangesAsync();
        }
    }
}


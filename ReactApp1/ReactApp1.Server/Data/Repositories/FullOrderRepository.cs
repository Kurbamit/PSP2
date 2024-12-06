using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public class FullOrderRepository : IFullOrderRepository
    {
        
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRepository> _logger;
        
        public FullOrderRepository(AppDbContext context, Logger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task AddItemToOrderAsync(FullOrderModel fullOrder)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                
                var newFullOrder = new FullOrder
                {
                    FullOrderId = fullOrder.FullOrderId,
                    OrderId = fullOrder.OrderId,
                    ItemId = fullOrder.ItemId,
                    Count = fullOrder.Count
                };
                
                await _context.Set<FullOrder>().AddAsync(newFullOrder);
                await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException($"An error occurred while adding fullOrder {fullOrder.FullOrderId} record to the database.", e);
            }
        }
        
        public async Task<FullOrderModel?> GetFullOrderByIdAsync(int fullOrderId)
        {
            var fullOrder = await _context.FullOrders
                .Where(f => f.FullOrderId == fullOrderId)
                .Select(f => new FullOrderModel(f))
                .FirstOrDefaultAsync();
            
            return fullOrder;
        }

        public async Task UpdateItemInOrderCountAsync(FullOrderModel fullOrder)
        {
            var existingFullOrder = await _context.FullOrders
                .Where(f => f.FullOrderId == fullOrder.FullOrderId)
                .FirstOrDefaultAsync();

            if (existingFullOrder == null)
            {
                _logger.LogWarning($"Full {fullOrder.FullOrderId} not found");
                return;
            }
            
            // update item's quantity by adding new count to existing count
            existingFullOrder.Count += fullOrder.Count;
            
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteItemFromOrderAsync(int itemId)
        {
            // TODO
            await Task.CompletedTask;
        }
    }
}


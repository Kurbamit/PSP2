using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public class FullOrderRepository : IFullOrderRepository
    {
        
        private readonly AppDbContext _context;
        private readonly ILogger<FullOrderRepository> _logger;
        
        public FullOrderRepository(AppDbContext context, ILogger<FullOrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task AddItemToOrderAsync(FullOrderModel fullOrder)
        {
            try
            {
                var newFullOrder = new FullOrder
                {
                    OrderId = fullOrder.OrderId,
                    ItemId = fullOrder.ItemId,
                    Count = fullOrder.Count
                };
                
                await _context.Set<FullOrder>().AddAsync(newFullOrder);
                await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException($"An error occurred while adding fullOrder (orderId ={fullOrder.OrderId}, itemId = {fullOrder.ItemId}) record to the database.", e);
            }
        }
        
        public async Task<FullOrderModel?> GetFullOrderAsync(int orderId, int itemId)
        {
            return await _context.FullOrders
                .Where(f => f.OrderId == orderId && f.ItemId == itemId)
                .Select(f => new FullOrderModel(f))
                .FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetOrderItemsAsync(int orderId)
        {
            return await _context.FullOrders
                .Where(f => f.OrderId == orderId)
                .Select(f => f.ItemId)
                .ToListAsync();
        }
        

        public async Task UpdateItemInOrderCountAsync(FullOrderModel fullOrder)
        {
            var existingFullOrder = await _context.FullOrders
                .Where(f => f.OrderId == fullOrder.OrderId && f.ItemId == fullOrder.ItemId)
                .FirstOrDefaultAsync();

            if (existingFullOrder == null)
            {
                _logger.LogInformation($"No existing FullOrder found for Order {fullOrder.OrderId} and Item {fullOrder.ItemId}");
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


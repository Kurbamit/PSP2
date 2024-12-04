using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

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
            var existingFullOrder = await _context.FullOrders.FirstOrDefaultAsync(o => o.FullOrderId == fullOrder.FullOrderId);

            if (existingFullOrder == null)
            {
                _logger.LogInformation($"Item {fullOrder.ItemId} is already associated with the order {fullOrder.OrderId}");
                return;
            }
            
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
            throw new Exception($"An error occurred while adding fullOrder {fullOrder.FullOrderId} to the database.", e);
        }
    }
}
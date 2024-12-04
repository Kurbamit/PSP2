using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(AppDbContext context, Logger<OrderRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task AddOrderAsync(OrderModel order)
    {
        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            if (existingOrder != null)
            {
                _logger.LogInformation($"Order {order.OrderId} already exists");
                return;
            }

            await CreateNewOrder();
        }
        catch (DbUpdateException e)
        {
            throw new Exception($"An error occurred while adding new order {order.OrderId} to the database.", e);
        }

        async Task CreateNewOrder()
        {
            var newOrder = new Order
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CreatedByEmployeeId = order.CreatedByEmployeeId,
                ReceiveTime = order.ReceiveTime,
                DiscountPercentage = order.DiscountPercentage,  // Can be null
                DiscountFixed = order.DiscountFixed,            // Can be null
                PaymentId = order.PaymentId,                    // Can be null
                Refunded = order.Refunded,
                ReservationId = order.ReservationId             // Can be null
            };
            
            await _context.Set<Order>().AddAsync(newOrder);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PaginatedResult<OrderModel>> GetAllOrdersAsync(int pageNumber, int pageSize)
    {
        var totalOrders = await _context.Set<Order>().CountAsync();
        var totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);

        var orders = await _context.Set<Order>()
            .OrderBy(o => o.OrderId)
            .Paginate(pageNumber, pageSize)
            .Select(o => new OrderModel(o))
            .ToListAsync();
        
        return new PaginatedResult<OrderModel>
        {
            Items = orders,
            TotalPages = totalPages,
            TotalItems = totalOrders,
            CurrentPage = pageNumber
        };
    }
    
    public async Task<OrderModel?> GetOrderByIdAsync(int orderId)
    {
        var item = await _context.Orders
            .Where(order => order.OrderId == orderId)
            .Select(o => new OrderModel(o))
            .FirstOrDefaultAsync();
        
        return item;
    }
}
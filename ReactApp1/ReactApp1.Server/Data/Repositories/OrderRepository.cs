using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(AppDbContext context, Logger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OrderModel> AddEmptyOrderAsync(int createdByEmployeeId)
        {
            try
            {
                var emptyOrder = new Order
                {
                    Status = (int)OrderStatusEnum.Open,
                    CreatedByEmployeeId = createdByEmployeeId,
                    ReceiveTime = DateTime.Now,
                    DiscountPercentage = null,
                    DiscountFixed = null,
                    PaymentId = null,
                    Refunded = false,
                    ReservationId = null
                };

                var orderEntity = await _context.Set<Order>().AddAsync(emptyOrder);
                await _context.SaveChangesAsync();
                
                return new OrderModel(orderEntity.Entity);
            }
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new order to the database.", e);
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

        public async Task<OrderModel?> AddDiscountToOrderAsync()
        {
            // TODO
            return await Task.FromResult<OrderModel?>(null);
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            // TODO
            await Task.CompletedTask;
        }
    }
    
}

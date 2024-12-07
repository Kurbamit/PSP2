using System.Data.Common;
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

        public OrderRepository(AppDbContext context, ILogger<OrderRepository> logger)
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
                    ReceiveTime = DateTime.UtcNow,
                    DiscountPercentage = null,
                    DiscountFixed = null,
                    PaymentId = null,
                    Refunded = false,
                    ReservationId = null
                };

                var orderEntity = await _context.Orders.AddAsync(emptyOrder);
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
            var totalOrders = await _context.Orders.CountAsync();
            var totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);

            var orders = await _context.Orders
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
            var order = await _context.Orders
                .Where(order => order.OrderId == orderId)
                .Select(o => new OrderModel(o))
                .FirstOrDefaultAsync();

            return order;
        }
        public async Task DeleteOrderAsync(int orderId)
        {
            var existingOrder = await _context.Orders
                .Where(order => order.OrderId == orderId)
                .FirstOrDefaultAsync();
            
            if(existingOrder == null)
                throw new InvalidOperationException($"The specified order {orderId} does not exist");

            try
            {
                _context.Orders.Remove(existingOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbException e)
            {
                throw new DbUpdateException($"An error occurred while deleting order (orderId = {orderId} record to the database.", e);
            }
        }

        public async Task UpdateOrderAsync(OrderModel order)
        {
            var existingOrder = await _context.Orders
                .Where(o => o.OrderId == order.OrderId)
                .FirstOrDefaultAsync();
            
            if(existingOrder == null)
                throw new InvalidOperationException($"The specified order {order.OrderId} does not exist");
            
            try
            {
                _context.Orders.Update(new Order
                {
                    Status = order.Status,
                    CreatedByEmployeeId = order.CreatedByEmployeeId,
                    ReceiveTime = order.ReceiveTime,
                    DiscountPercentage = order.DiscountPercentage,
                    DiscountFixed = order.DiscountFixed,
                    PaymentId = order.PaymentId,
                    Refunded = order.Refunded,
                    ReservationId = order.ReservationId
                });
                await _context.SaveChangesAsync();
            }   
            catch (DbUpdateException e)
            {
                throw new DbUpdateException($"An error occurred while updating order (orderId = {order.OrderId}) record in the database.", e);
            }
        }
    }
}

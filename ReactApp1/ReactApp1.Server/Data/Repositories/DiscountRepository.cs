using Microsoft.EntityFrameworkCore;

namespace ReactApp1.Server.Data.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FullOrderRepository> _logger;
        
        public DiscountRepository(AppDbContext context, ILogger<FullOrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<decimal> GetDiscountAsync(int discountId)
        {
            return await _context.Discounts
                .Where(d => d.DiscountId == discountId)
                .Select(d => d.Percentage)
                .FirstOrDefaultAsync();
        }
    }
}
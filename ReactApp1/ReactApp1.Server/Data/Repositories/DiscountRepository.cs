using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models.Models.Domain;

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
        
        public async Task<DiscountModel> GetDiscountAsync(int discountId)
        {
            return await _context.Discounts
                .Where(d => d.DiscountId == discountId)
                .Select(d => new DiscountModel()
                {
                    DiscountId = d.DiscountId,
                    DiscountName = d.Name,
                    Value = d.Percentage,
                })
                .FirstOrDefaultAsync();
        }
    }
}
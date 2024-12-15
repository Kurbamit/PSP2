using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Data.Repositories
{
    public class SharedSearchesRepository : ISharedSearchesRepository
    {
        private readonly AppDbContext _context;
        
        public SharedSearchesRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<SharedItem>> GetAllItems(int establishmentId, string? search)
        {
            var result = await _context.Items
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Name.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedItem()
                {
                    Id = f.ItemId,
                    Name = f.Name == null ? "None" : f.Name
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            return result;
        }
        
        public async Task<List<SharedItem>> GetAllDiscounts(int establishmentId, string? search)
        {
            var result = await _context.Discounts
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Name.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedItem()
                {
                    Id = f.DiscountId,
                    Name = f.Name == null ? "None" : f.Name + " - " + f.Percentage + "%"
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            return result;
        }
    }
}
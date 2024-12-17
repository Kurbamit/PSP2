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
                .Where(f => 
                    (!f.ValidFrom.HasValue || f.ValidFrom.Value <= DateTime.UtcNow) &&
                    (!f.ValidTo.HasValue || f.ValidTo.Value >= DateTime.UtcNow)
                )
                .Select(f => new SharedItem()
                {
                    Id = f.DiscountId,
                    Name = f.Name == null ? "None" : f.Name + " - " + f.Percentage + "%"
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            return result;
        }

        public async Task<List<SharedService>> GetAllServices(int establishmentId, string? search)
        {
            var result = await _context.Services
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Name.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedService()
                {
                    Id = f.ServiceId,
                    Name = f.Name == null ? "None" : f.Name
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            return result;
        }
        public async Task<List<SharedItem>> GetAllTaxes(string? search)
        {
            var result = await _context.Taxes
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Description.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedItem()
                {
                    Id = f.TaxId,
                    Name = f.Description == null ? "None" : f.Description
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            return result;
        }
        public async Task<List<SharedItem>> GetAllBaseItems(int establishmentId, string? search)
        {
            var result = await _context.Items
                .Where(f => f.BaseItemId == 0)
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Name.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedItem()
                {
                    Id = f.ItemId,
                    Name = f.Name == null ? "None" : f.Name
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            var newEntry = new SharedItem()
            {
                Id = 0,
                Name = "Make item base item"
            };

            result.Insert(0, newEntry);

            return result;
        }
    }
}
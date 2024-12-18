using System.Security.Principal;
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
        
        public async Task<List<SharedItem>> GetAllItems(int establishmentId, string? search, IPrincipal user)
        {
            var result = await _context.Items
                .FilterByAuthorizedUser(user)
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Name.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedItem()
                {
                    Id = f.ItemId,
                    Name = f.Name == null ? "None" : f.Name
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            return result;
        }
        
        public async Task<List<SharedItem>> GetAllDiscounts(int establishmentId, string? search, IPrincipal user)
        {
            var result = await _context.Discounts
                .FilterByAuthorizedUser(user)
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

        public async Task<List<SharedService>> GetAllServices(int establishmentId, string? search, IPrincipal user)
        {
            var result = await _context.Services
                .FilterByAuthorizedUser(user)
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Name.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedService()
                {
                    Id = f.ServiceId,
                    Name = f.Name == null ? "None" : f.Name
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            return result;
        }

        public async Task<List<SharedEmployee>> GetAllEmployees(int establishmentId, string? search, IPrincipal user)
        {
            var result = await _context.Employees
                .FilterByAuthorizedUser(user)
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.FirstName.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedEmployee()
                {
                    Id = f.EmployeeId,
                    Name = f.FirstName + " " + f.LastName
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
        public async Task<List<SharedItem>> GetAllBaseItemsForEdit(string? search, IPrincipal user)
        {
            var result = await _context.Items
                .FilterByAuthorizedUser(user)
                .Where(f => f.BaseItemId == 0)
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Name.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedItem()
                {
                    Id = f.ItemId,
                    Name = f.Name == null ? "None" : f.Name
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            return result;
        }
        public async Task<List<SharedItem>> GetAllBaseItems(string? search, IPrincipal user)
        {
            var result = await _context.Items
                .FilterByAuthorizedUser(user)
                .Where(f => f.BaseItemId == 0)
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Name.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedItem()
                {
                    Id = f.ItemId,
                    Name = f.Name == null ? "None" : f.Name
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            return result;
        }
        public async Task<List<SharedItem>> GetAllItemsVariations(string? search, int itemId, IPrincipal user)
        {
            var result = await _context.Items
                .FilterByAuthorizedUser(user)
                .Where(f => f.BaseItemId == itemId || f.ItemId == itemId)
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Name.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedItem()
                {
                    Id = f.ItemId,
                    Name = f.Name == null ? "None" : f.Name
                }).OrderBy(f => f.Name.ToLower()).ToListAsync();

            return result;
        }
    }
}
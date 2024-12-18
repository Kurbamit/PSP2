using System.Security.Principal;
using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Services
{
    public interface ISharedSearchesService
    {
        public Task<List<SharedItem>> GetAllItems(int? establishmentId, string? search, IPrincipal user);
        public Task<List<SharedService>> GetAllServices(int? establishmentId, string? search, IPrincipal user);
        public Task<List<SharedItem>> GetAllDiscounts(int? establishmentId, string? search, IPrincipal user);
        public Task<List<SharedEmployee>> GetAllEmployees(int? establishmentId, string? search, IPrincipal user);
        public Task<List<SharedItem>> GetAllTaxes(string? search);
        public Task<List<SharedItem>> GetAllBaseItemsForEdit(int? establishmentId, string? search, IPrincipal user);
        public Task<List<SharedItem>> GetAllBaseItems(int? establishmentId, string? search, IPrincipal user);
        public Task<List<SharedItem>> GetAllItemsVariations(int? establishmentId, string? search, int itemId, IPrincipal user);
    }
}
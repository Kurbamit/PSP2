using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Services
{
    public interface ISharedSearchesService
    {
        public Task<List<SharedItem>> GetAllItems(int? establishmentId, string? search);
        public Task<List<SharedService>> GetAllServices(int? establishmentId, string? search);
        public Task<List<SharedItem>> GetAllDiscounts(int? establishmentId, string? search);
    }
}
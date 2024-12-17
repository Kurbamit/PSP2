using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface ITaxRepository
    {
        Task<PaginatedResult<Tax>> GetAllTaxesAsync(int pageNumber, int pageSize);
        Task<TaxModel?> GetTaxByIdAsync(int taxId);
        Task AddTaxAsync(TaxModel tax);
        Task UpdateTaxAsync(TaxModel tax);
        Task DeleteTaxAsync(int taxId);
        Task AddItemTaxAsync(ItemTaxModel tax);
        Task AddServiceTaxAsync(ServiceTaxModel tax);
        Task RemoveItemTaxAsync(ItemTaxModel tax);
        Task RemoveServiceTaxAsync(ServiceTaxModel tax);
        Task<List<TaxModel>> GetItemTaxesAsync(int itemId);
        Task<List<TaxModel>> GetServiceTaxesAsync(int serviceId);

    }
}


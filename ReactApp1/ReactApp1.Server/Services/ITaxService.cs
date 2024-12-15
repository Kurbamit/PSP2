using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface ITaxService
    {
        Task<PaginatedResult<Tax>> GetAllTaxes(int pageNumber, int pageSize);
        Task<TaxModel?> GetTaxById(int taxId);
        Task AddTax(TaxModel tax);
        Task UpdateTax(TaxModel tax);
        Task AddItemTax(ItemTax tax);
        Task AddServiceTax(ServiceTax tax);
        Task RemoveItemTax(ItemTax tax);
        Task RemoveServiceTax(ServiceTax tax);
        Task<List<Tax>> GetItemTaxes(int itemId);
        Task<List<Tax>> GetServiceTaxes(int serviceId);

    }
}

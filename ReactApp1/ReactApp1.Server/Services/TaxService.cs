using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class TaxesService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;

        public TaxesService(ITaxRepository taxRepository)
        {
            _taxRepository = taxRepository;
        }

        public Task<PaginatedResult<Tax>> GetAllTaxes(int pageNumber, int pageSize)
        {
            return _taxRepository.GetAllTaxesAsync(pageNumber, pageSize);
        }
        public Task<TaxModel?> GetTaxById(int taxId)
        {
            return _taxRepository.GetTaxByIdAsync(taxId);
        }
        public Task AddTax(TaxModel tax)
        {
            return _taxRepository.AddTaxAsync(tax);
        }
        public Task UpdateTax(TaxModel tax)
        {
            return _taxRepository.UpdateTaxAsync(tax);
        }
        public Task AddItemTax(ItemTax tax)
        {
            return _taxRepository.AddItemTaxAsync(tax);
        }
        public Task AddServiceTax(ServiceTax tax)
        {
            return _taxRepository.AddServiceTaxAsync(tax);
        }
        public Task RemoveItemTax(ItemTax tax)
        {
            return _taxRepository.RemoveItemTaxAsync(tax);
        }
        public Task RemoveServiceTax(ServiceTax tax)
        {
            return _taxRepository.RemoveServiceTaxAsync(tax);
        }
        public Task<List<Tax>> GetItemTaxes(int itemId)
        {
            return _taxRepository.GetItemTaxesAsync(itemId);
        }
        public Task<List<Tax>> GetServiceTaxes(int serviceId)
        {
            return _taxRepository.GetServiceTaxesAsync(serviceId);
        }

    }
}


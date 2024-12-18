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
        public Task DeleteTax(int taxId)
        {
            return _taxRepository.DeleteTaxAsync(taxId);
        }

        public Task AddItemTax(ItemTaxModel tax)
        {
            return _taxRepository.AddItemTaxAsync(tax);
        }
        public Task AddServiceTax(ServiceTaxModel tax)
        {
            return _taxRepository.AddServiceTaxAsync(tax);
        }
        public Task RemoveItemTax(ItemTaxModel tax)
        {
            return _taxRepository.RemoveItemTaxAsync(tax);
        }
        public Task RemoveServiceTax(ServiceTaxModel tax)
        {
            return _taxRepository.RemoveServiceTaxAsync(tax);
        }
        public Task<List<TaxModel>> GetItemTaxes(int itemId)
        {
            return _taxRepository.GetItemTaxesAsync(itemId);
        }
        public Task<List<TaxModel>> GetServiceTaxes(int serviceId)
        {
            return _taxRepository.GetServiceTaxesAsync(serviceId);
        }

    }
}


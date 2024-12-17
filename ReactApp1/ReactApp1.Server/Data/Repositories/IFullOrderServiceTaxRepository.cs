using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

public interface IFullOrderServiceTaxRepository
{
    Task<List<TaxModel>> GetFullOrderServiceTaxesAsync(int fullOrderId);
    Task DeleteItemFromFullOrderServiceTaxAsync(FullOrderServiceTaxModel fullOrderServiceTax);
    Task AddItemToFullOrderServiceTaxAsync(FullOrderServiceTaxModel fullOrderServiceTax);
}
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories;

public interface IFullOrderTaxRepository
{
    Task<List<TaxModel>> GetFullOrderItemTaxesAsync(int fullOrderId);
    Task DeleteItemFromFullOrderTaxAsync(FullOrderTaxModel fullOrderTax);
    Task AddItemToFullOrderTaxAsync(FullOrderTaxModel fullOrderTax);
}
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Exceptions.ItemExceptions;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Migrations;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
using Stripe;

namespace ReactApp1.Server.Data.Repositories
{
    public class TaxRepository : ITaxRepository
    {
        private readonly AppDbContext _context;

        public TaxRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Tax>> GetAllTaxesAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Set<Tax>().CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var taxes = await _context.Set<Tax>()
                .OrderBy(tax => tax.TaxId)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();

            return new PaginatedResult<Tax>
            {
                Items = taxes,
                TotalPages = totalPages,
                TotalItems = totalItems,
                CurrentPage = pageNumber
            };
        }
        public async Task<TaxModel?> GetTaxByIdAsync(int taxId)
        {
            var tax = await _context.Taxes
            .Where(f => f.TaxId == taxId)
            .Select(f => new TaxModel()
            {
                TaxId = f.TaxId,
                Description = f.Description,
                Percentage = f.Percentage,
            }).FirstOrDefaultAsync();

            return tax;
        }
        public async Task AddTaxAsync(TaxModel tax)
        {
            try
            {
                var newTax = new Tax
                {
                    Description = tax.Description,
                    Percentage = tax.Percentage,
                };

                var taxEntity = await _context.Taxes.AddAsync(newTax);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new tax to the database.", e);
            }
        }
        public async Task UpdateTaxAsync(TaxModel tax)
        {
            try
            {
                var existingTax = await _context.Set<Tax>()
                    .FirstOrDefaultAsync(i => i.TaxId == tax.TaxId);


                if (existingTax == null)
                {
                    throw new KeyNotFoundException($"Tax with ID {tax.TaxId} not found.");
                }

                tax.MapUpdate(existingTax);
                _context.Set<Tax>().Update(existingTax);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating the service: {tax.TaxId}.", e);
            }
        }
        public async Task DeleteTaxAsync(int taxId)
        {
            try
            {
                _context.Set<Tax>().Remove(new Tax
                {
                    TaxId = taxId
                });

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the tax {taxId} from the database.", e);
            }
        }
        public async Task AddItemTaxAsync(ItemTaxModel tax)
        {
            try
            {
                var newTax = new ItemTax
                {
                    TaxId = tax.TaxId,
                    ItemId = tax.ItemId,
                };

                var taxEntity = await _context.ItemTaxes.AddAsync(newTax);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new item tax to the database.", e);
            }
        }
        public async Task AddServiceTaxAsync(ServiceTaxModel tax)
        {
            try
            {
                var newTax = new ServiceTax
                {
                    TaxId = tax.TaxId,
                    ServiceId = tax.ServiceId,
                };

                var taxEntity = await _context.ServiceTaxes.AddAsync(newTax);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new service tax to the database.", e);
            }
        }
        public async Task RemoveItemTaxAsync(ItemTaxModel tax)
        {
            try
            {
                var itemTax = await _context.Set<ItemTax>()
                    .FirstOrDefaultAsync(it => it.ItemId == tax.ItemId && it.TaxId == tax.TaxId);

                if (itemTax == null)
                {
                    throw new Exception($"No ItemTax record found for ItemId {tax.ItemId} and TaxId {tax.TaxId}.");
                }

                _context.Set<ItemTax>().Remove(itemTax);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the item tax {tax.TaxId} from the database.", e);
            }
        }
        public async Task RemoveServiceTaxAsync(ServiceTaxModel tax)
        {
            try
            {
                var serviceTax = await _context.Set<ServiceTax>()
                    .FirstOrDefaultAsync(it => it.ServiceId == tax.ServiceId && it.TaxId == tax.TaxId);

                if (serviceTax == null)
                {
                    throw new Exception($"No ItemTax record found for ItemId {tax.ServiceId} and TaxId {tax.TaxId}.");
                }

                _context.Set<ServiceTax>().Remove(serviceTax);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the service tax {tax.TaxId} from the database.", e);
            }
        }
        public async Task<List<Tax>> GetItemTaxesAsync(int itemId)
        {
            var taxes = await _context.Set<ItemTax>()
                .Where(it => it.ItemId == itemId)
                .Select(it => it.Tax)
                .ToListAsync();

            return taxes;
        }
        public async Task<List<Tax>> GetServiceTaxesAsync(int serviceId)
        {
            var taxes = await _context.Set<ServiceTax>()
                .Where(it => it.ServiceId == serviceId)
                .Select(it => it.Tax)
                .ToListAsync();

            return taxes;
        }
    }
}


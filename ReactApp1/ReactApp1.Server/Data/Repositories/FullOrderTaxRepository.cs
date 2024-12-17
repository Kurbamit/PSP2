using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Exceptions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApp1.Server.Data.Repositories
{
    public class FullOrderTaxRepository : IFullOrderTaxRepository
    {
        private readonly AppDbContext _context;

        public FullOrderTaxRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaxModel>> GetFullOrderItemTaxesAsync(int fullOrderId)
        {
            try
            {
                var taxes = await _context.FullOrderTaxes
                    .Where(t => t.FullOrderId == fullOrderId)
                    .Select(t => new TaxModel
                    {
                        Percentage = t.Percentage,
                        Description = t.Description
                    })
                    .ToListAsync();

                return taxes;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving taxes for full order service {fullOrderId}.");
            }
        }

        public async Task DeleteItemFromFullOrderTaxAsync(FullOrderTaxModel fullOrderTax)
        {
            try
            {
                var taxToDelete = await _context.FullOrderTaxes
                .Where(t => t.FullOrderTaxId == fullOrderTax.FullOrderTaxId)
                .FirstOrDefaultAsync();


                _context.FullOrderTaxes.Remove(taxToDelete);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException($"An error occurred while deleting tax {fullOrderTax.FullOrderTaxId}.", e);
            }
        }

        public async Task AddItemToFullOrderTaxAsync(FullOrderTaxModel fullOrderTax)
        {
            try
            {
                var newTax = new FullOrderTax
                {
                    FullOrderId = fullOrderTax.FullOrderId,
                    Percentage = fullOrderTax.Percentage,
                    Description = fullOrderTax.Description
                };

                await _context.FullOrderTaxes.AddAsync(newTax);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException("An error occurred while adding a new tax.", e);
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Exceptions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApp1.Server.Data.Repositories
{
    public class FullOrderServiceTaxRepository : IFullOrderServiceTaxRepository
    {
        private readonly AppDbContext _context;

        public FullOrderServiceTaxRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaxModel>> GetFullOrderServiceTaxesAsync(int fullOrderId)
        {
            try
            {
                var taxes = await _context.FullOrderServiceTaxes
                    .Where(t => t.FullOrderServiceId == fullOrderId)
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

        // Method to delete a specific tax from a service within an order
        public async Task DeleteItemFromFullOrderServiceTaxAsync(FullOrderServiceTaxModel fullOrderServiceTax)
        {
            try
            {
                var taxToDelete = await _context.FullOrderServiceTaxes
                .Where(t => t.FullOrderServiceTaxId == fullOrderServiceTax.FullOrderServiceTaxId)
                .FirstOrDefaultAsync();


                _context.FullOrderServiceTaxes.Remove(taxToDelete);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException($"An error occurred while deleting tax {fullOrderServiceTax.FullOrderServiceTaxId}.", e);
            }
        }

        // Method to add a new tax to a service within an order
        public async Task AddItemToFullOrderServiceTaxAsync(FullOrderServiceTaxModel fullOrderServiceTax)
        {
            try
            {
                var newTax = new FullOrderServiceTax
                {
                    FullOrderServiceId = fullOrderServiceTax.FullOrderServiceId,
                    Percentage = fullOrderServiceTax.Percentage,
                    Description = fullOrderServiceTax.Description
                };

                await _context.FullOrderServiceTaxes.AddAsync(newTax);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException("An error occurred while adding a new tax.", e);
            }
        }
    }
}

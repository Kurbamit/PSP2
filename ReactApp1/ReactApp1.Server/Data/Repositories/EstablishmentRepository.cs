using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly AppDbContext _context;

        public EstablishmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Establishment>> GetAllEstablishmentsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Set<Establishment>().CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var establishments = await _context.Set<Establishment>()
                .OrderBy(establishment => establishment.EstablishmentId)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();

            return new PaginatedResult<Establishment>
            {
                Items = establishments,
                TotalPages = totalPages,
                TotalItems = totalItems,
                CurrentPage = pageNumber
            };
        }

        public async Task<EstablishmentModel?> GetEstablishmentByIdAsync(int establishmentId)
        {
            var establishment = await _context.Establishments
                .Where(f => f.EstablishmentId == establishmentId)
                .Select(f => new EstablishmentModel()
                {
                    EstablishmentId = f.EstablishmentId,
                    EstablishmentAddressId = f.EstablishmentAddressId,
                    Type = f.Type,
                    Country = f.EstablishmentAddress.Country,
                    City = f.EstablishmentAddress.City,
                    Street = f.EstablishmentAddress.Street,
                    StreetNumber = f.EstablishmentAddress.StreetNumber,
                }).FirstOrDefaultAsync();

            return establishment;
        }

        public async Task AddEstablishmentAsync(EstablishmentModel establishment, int? establishmentId)
        {
            if (!establishmentId.HasValue)
            {
                throw new ArgumentNullException(nameof(establishmentId));
            }
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var newEstablishment = new Establishment
                {
                    Type = establishment.Type,
                    EstablishmentAddressId = establishment.EstablishmentAddressId
                };
                
                await _context.Establishments.AddAsync(newEstablishment);
                await _context.SaveChangesAsync();

                var address = new EstablishmentAddress
                {
                    Country = establishment.Country,
                    City = establishment.City,
                    Street = establishment.Street,
                    StreetNumber = establishment.StreetNumber,
                    EstablishmentId = newEstablishment.EstablishmentId
                };

                await _context.EstablishmentAddresses.AddAsync(address);
                await _context.SaveChangesAsync();

                newEstablishment.EstablishmentAddress = address;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new establishment to the database.", e);
            }
        }

        public async Task UpdateEstablishmentAsync(EstablishmentModel establishment)
        {
            try
            {
                var existingEstablishment = await _context.Set<Establishment>()
                    .Include(e => e.EstablishmentAddress)
                    .FirstOrDefaultAsync(e => e.EstablishmentId == establishment.EstablishmentId);

                if (existingEstablishment == null)
                {
                    throw new KeyNotFoundException($"Establishment with ID {establishment.EstablishmentId} not found.");
                }

                establishment.MapUpdate(existingEstablishment);

                _context.Set<Establishment>().Update(existingEstablishment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating the establishment: {establishment.EstablishmentId}.", e);
            }
        }

        public async Task DeleteEstablishmentAsync(int establishmentId)
        {
            try
            {
                _context.Set<Establishment>().Remove(new Establishment
                {
                    EstablishmentId = establishmentId
                });

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the establishment {establishmentId} from the database.", e);
            }
        }

        private string GeneratePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
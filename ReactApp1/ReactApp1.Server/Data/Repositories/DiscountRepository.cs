using System.Net;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Exceptions;
using ReactApp1.Server.Exceptions.GiftCardExceptions;
using ReactApp1.Server.Exceptions.ItemExceptions;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FullOrderRepository> _logger;
        
        public DiscountRepository(AppDbContext context, ILogger<FullOrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<PaginatedResult<DiscountModelForAPI>> GetAllDiscounts(int pageSize, int pageNumber, IPrincipal user)
        {
            var totalItems = await _context.Set<Discount>().FilterByAuthorizedUser(user).CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            var discounts = _context.Discounts
                .FilterByAuthorizedUser(user)
                .Select(d => new DiscountModelForAPI()
                {
                    DiscountId = d.DiscountId,
                    DiscountName = d.Name,
                    Value = d.Percentage,
                    ValidFrom = d.ValidFrom,
                    ValidTo = d.ValidTo,
                    EstablishmentId = d.EstablishmentId
                })
                .Paginate(pageSize, pageNumber);

            return new PaginatedResult<DiscountModelForAPI>()
            {
                Items = discounts,
                TotalPages = totalPages,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
            };
        }

        public async Task<DiscountModelForAPI> GetDiscountByIdAsync(int discountId, IPrincipal user)
        {
            var discount = await _context.Discounts
                .FilterByAuthorizedUser(user)
                .Where(d => d.DiscountId == discountId)
                .Select(d => new DiscountModelForAPI()
                {
                    DiscountId = d.DiscountId,
                    DiscountName = d.Name,
                    Value = d.Percentage,
                    ValidFrom = d.ValidFrom,
                    ValidTo = d.ValidTo,
                    EstablishmentId = d.EstablishmentId
                }).FirstOrDefaultAsync();

            if (discount == null)
            {
                throw new AuthorizationException();
            }
            
            return discount;
        }

        public async Task<int> CreateDiscount(DiscountModelForAPI discount, IPrincipal user)
        {
            var establishmentId = user.GetUserEstablishmentId();
            if (establishmentId == null)
            {
                throw new AuthorizationException();
            }

            try
            {
                var newDiscount = new Discount();
            
                newDiscount.Name = discount.DiscountName;
                newDiscount.Percentage = discount.Value;
                newDiscount.ValidFrom = discount.ValidFrom;
                newDiscount.ValidTo = discount.ValidTo;
                newDiscount.EstablishmentId = establishmentId.Value;
                newDiscount.ReceiveTime = DateTime.UtcNow;
            
                await _context.Discounts.AddAsync(newDiscount);
                await _context.SaveChangesAsync();

                return newDiscount.DiscountId;
            } catch (DbUpdateException e)
            {
                _logger.LogError(e, "Failed to create discount");
                throw new CreateDiscountException();
            }
        }

        public async Task UpdateDiscount(DiscountModelForAPI discount)
        {
            var dbDiscount = await _context.Discounts
                .Where(d => d.DiscountId == discount.DiscountId)
                .FirstOrDefaultAsync();

            if (dbDiscount == null)
            {
                throw new ItemNotFoundException(discount.DiscountId);
            }

            dbDiscount.Name = discount.DiscountName;
            dbDiscount.Percentage = discount.Value;
            dbDiscount.ValidFrom = discount.ValidFrom;
            dbDiscount.ValidTo = discount.ValidTo;

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException e)
            {
                _logger.LogError(e, "Failed to update discount");
                throw new UpdateDiscountException();
            }
        }

        public async Task DeleteDiscount(int discountId, IPrincipal user)
        {
            var discount = await _context.Discounts
                .FilterByAuthorizedUser(user)
                .Where(d => d.DiscountId == discountId)
                .FirstOrDefaultAsync();

            if (await _context.Orders.AnyAsync(f => f.DiscountId == discountId))
            {
                throw new DiscountInUseException(discountId);
            }
            
            if (discount == null)
            {
                throw new DiscountNotFoundException(discountId);
            }
            
            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
        }
        
        public async Task<DiscountModel> GetDiscountAsync(int discountId)
        {
            return await _context.Discounts
                .Where(d => d.DiscountId == discountId)
                .Select(d => new DiscountModel()
                {
                    DiscountId = d.DiscountId,
                    DiscountName = d.Name,
                    Value = d.Percentage,
                })
                .FirstOrDefaultAsync();
        }
    }
}
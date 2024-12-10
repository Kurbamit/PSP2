using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReactApp1.Server.Data.Repositories
{
    public class GiftCardRepository : IGiftCardRepository
    {
        private readonly AppDbContext _context;

        public GiftCardRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<PaginatedResult<GiftCard>> GetAllGiftCardsAsync(int pageNumber, int pageSize)
        {
            var totalGiftCard= await _context.Set<GiftCard>().CountAsync();
            var totalPages = (int)Math.Ceiling(totalGiftCard / (double)pageSize);
            
            var giftCards = await _context.Set<GiftCard>()
                .OrderBy(giftcard => giftcard.GiftCardId)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();
            
            return new PaginatedResult<GiftCard>
            {
                Items = giftCards,
                TotalPages = totalPages,
                TotalItems = totalGiftCard,
                CurrentPage = pageNumber
            };
        }

        public async Task<GiftCardModel?> GetGiftCardByIdAsync(int giftCardId)
        {
            var giftCard = await _context.GiftCards
                .Where(f => f.GiftCardId == giftCardId)
                .Select(f => new GiftCardModel()
                {
                    GiftCardId = f.GiftCardId,
                    ExpirationDate = f.ExpirationDate,
                    Amount = f.Amount,
                    Code = f.Code,
                    ReceiveTime = f.ReceiveTime,
                    PaymentId = f.PaymentId
                }).FirstOrDefaultAsync();

            return giftCard;
        }
        public async Task<GiftCardModel?> GetGiftCardByCodeAsync(string giftCardCode)
        {
            var giftCard = await _context.GiftCards
                .Where(f => f.Code == giftCardCode)
                .Select(f => new GiftCardModel()
                {
                    GiftCardId = f.GiftCardId,
                    ExpirationDate = f.ExpirationDate,
                    Amount = f.Amount,
                    Code = f.Code,
                    ReceiveTime = f.ReceiveTime,
                    PaymentId = f.PaymentId
                }).FirstOrDefaultAsync();

            return giftCard;
        }
        public async Task AddGiftCardAsync(GiftCard giftCard)
        {
            try
            {
                await _context.Set<GiftCard>().AddAsync(giftCard);
                await _context.SaveChangesAsync();

            }   
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new giftcard to the database.", e);
            }
        }
        
        public async Task UpdateGiftCardAsync(GiftCardModel giftCard)
        {
            try
            {
                var existingGiftCard = await _context.Set<GiftCard>()
                    .FirstOrDefaultAsync(i => i.GiftCardId == giftCard.GiftCardId);
                
                
                if (existingGiftCard == null)
                {
                    throw new KeyNotFoundException($"GiftCard with ID {giftCard.GiftCardId} not found.");
                }

                giftCard.MapUpdate(existingGiftCard);

                _context.Set<GiftCard>().Update(existingGiftCard);
                await _context.SaveChangesAsync();
            }   
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating the giftcard: {giftCard.GiftCardId}.", e);
            }
        }
        
        public async Task DeleteGiftCardAsync(int giftCardId)
        {
            try
            {
                _context.Set<GiftCard>().Remove(new GiftCard
                {
                    GiftCardId = giftCardId
                });
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the giftcard {giftCardId} from the database.", e);
            }
        }
    }
}


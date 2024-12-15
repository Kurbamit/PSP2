using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IGiftCardRepository
    {
        Task<PaginatedResult<GiftCard>> GetAllGiftCardsAsync(int pageNumber, int pageSize);
        Task<GiftCardModel?> GetGiftCardByIdAsync(int giftCardId);
        Task<GiftCardModel?> GetGiftCardByCodeAsync(string giftCard);
        Task AddGiftCardAsync(GiftCardModel giftCard);
        Task UpdateGiftCardAsync(GiftCardModel giftCard);
        Task DeleteGiftCardAsync(int giftCardId);
    }
}


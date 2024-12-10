using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IGiftCardService
{
    Task<PaginatedResult<GiftCard>> GetAllGiftCards(int pageSize, int pageNumber);
    Task<GiftCardModel?> GetGiftCardById(int giftCardId);
    Task<GiftCardModel?> GetGiftCardByCode(string giftCardCode);
    Task CreateNewGiftCard(GiftCard giftCard);
    Task UpdateGiftCard(GiftCardModel giftCard);
    Task DeleteGiftCard(int giftCardId);
}
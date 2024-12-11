using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class GiftCardService : IGiftCardService
    {
        private readonly IGiftCardRepository _giftCardRepository;

        public GiftCardService(IGiftCardRepository giftCardRepository)
        {
            _giftCardRepository = giftCardRepository;
        }

        public Task<PaginatedResult<GiftCard>> GetAllGiftCards(int pageSize, int pageNumber)
        {
            return _giftCardRepository.GetAllGiftCardsAsync(pageSize, pageNumber);
        }

        public Task<GiftCardModel?> GetGiftCardById(int giftCardId)
        {
            return _giftCardRepository.GetGiftCardByIdAsync(giftCardId);
        }
        public Task<GiftCardModel?> GetGiftCardByCode(string giftCardCode)
        {
            return _giftCardRepository.GetGiftCardByCodeAsync(giftCardCode);
        }
        public Task CreateNewGiftCard(GiftCard giftCard)
        {
            return _giftCardRepository.AddGiftCardAsync(giftCard);
        }

        public Task UpdateGiftCard(GiftCardModel giftCard)
        {
            return _giftCardRepository.UpdateGiftCardAsync(giftCard);
        }

        public Task DeleteGiftCard(int giftCardId)
        {
            return _giftCardRepository.DeleteGiftCardAsync(giftCardId);
        }
    }
}


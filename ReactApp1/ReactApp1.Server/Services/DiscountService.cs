using System.Security.Principal;
using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        
        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }

        public async Task<PaginatedResult<DiscountModelForAPI>> GetAllDiscounts(int pageSize, int pageNumber, IPrincipal user)
        {
            return await _discountRepository.GetAllDiscounts(pageSize, pageNumber, user);
        }
        
        public async Task<DiscountModelForAPI> GetDiscountByIdAsync(int discountId, IPrincipal user)
        {
            return await _discountRepository.GetDiscountByIdAsync(discountId, user);
        }

        public async Task<int> CreateDiscount(DiscountModelForAPI discount, IPrincipal user)
        {
            return await _discountRepository.CreateDiscount(discount, user);
        }
        
        public async Task UpdateDiscount(DiscountModelForAPI discount)
        {
            await _discountRepository.UpdateDiscount(discount);
        }

        public async Task DeleteDiscount(int discountId, IPrincipal user)
        {
            await _discountRepository.DeleteDiscount(discountId, user);
        }
    }
}


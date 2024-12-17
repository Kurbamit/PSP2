using System.Security.Principal;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IDiscountRepository
    {
        Task<PaginatedResult<DiscountModelForAPI>> GetAllDiscounts(int pageSize, int pageNumber, IPrincipal user);
        Task<DiscountModelForAPI> GetDiscountByIdAsync(int discountId, IPrincipal user);
        Task<int> CreateDiscount(DiscountModelForAPI discount, IPrincipal user);
        Task UpdateDiscount(DiscountModelForAPI discount);
        Task DeleteDiscount(int discountId, IPrincipal user);
        Task<DiscountModel> GetDiscountAsync(int discountId);
    }
}

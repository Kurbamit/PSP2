using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IDiscountRepository
    {
        Task<DiscountModel> GetDiscountAsync(int discountId);
    }
}

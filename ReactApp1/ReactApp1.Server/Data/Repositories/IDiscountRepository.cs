namespace ReactApp1.Server.Data.Repositories
{
    public interface IDiscountRepository
    {
        Task<decimal> GetDiscountAsync(int discountId);
    }
}

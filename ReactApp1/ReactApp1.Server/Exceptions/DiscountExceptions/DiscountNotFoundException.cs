using System.Net;

namespace ReactApp1.Server.Exceptions.ItemExceptions
{
    public class DiscountNotFoundException : BaseException
    {
        public DiscountNotFoundException(int discountId)
            : base($"Discount (id = {discountId}) was not found", HttpStatusCode.NotFound)
        {
        }
    }
}
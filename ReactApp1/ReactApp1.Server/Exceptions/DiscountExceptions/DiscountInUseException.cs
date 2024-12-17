using System.Net;

namespace ReactApp1.Server.Exceptions.ItemExceptions
{
    public class DiscountInUseException : BaseException
    {
        public DiscountInUseException(int discountId)
            : base($"Discount (id = {discountId}) is in use.", HttpStatusCode.BadRequest)
        {
        }
    }
}
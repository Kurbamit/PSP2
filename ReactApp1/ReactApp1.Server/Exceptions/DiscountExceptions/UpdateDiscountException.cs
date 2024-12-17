using System.Net;

namespace ReactApp1.Server.Exceptions.ItemExceptions
{
    public class UpdateDiscountException : BaseException
    {
        public UpdateDiscountException()
            : base("Error occured updating new discount.", HttpStatusCode.InternalServerError)
        {
        }
    }
}
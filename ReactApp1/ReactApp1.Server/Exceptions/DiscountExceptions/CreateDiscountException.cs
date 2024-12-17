using System.Net;

namespace ReactApp1.Server.Exceptions.ItemExceptions
{
    public class CreateDiscountException : BaseException
    {
        public CreateDiscountException()
            : base("Error occured creating new discount.", HttpStatusCode.InternalServerError)
        {
        }
    }
}
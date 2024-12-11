using System.Net;

namespace ReactApp1.Server.Exceptions.GiftCardExceptions
{
    public class GiftcardInvalidException : BaseException
    {
        public GiftcardInvalidException()
            : base("Gift card code is invalid or expired.")
        {
        }
    }
}
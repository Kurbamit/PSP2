using System.Net;

namespace ReactApp1.Server.Exceptions.GiftCardExceptions
{
    public class GiftcardInvalidException : BaseException
    {
        public GiftcardInvalidException(string code)
            : base($"Gift card {code} is invalid or expired.")
        {
        }
    }
}
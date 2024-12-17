using System.Net;

namespace ReactApp1.Server.Exceptions.GiftCardExceptions
{
    public class AuthorizationException : BaseException
    {
        public AuthorizationException()
            : base($"Not found or not authorized.", HttpStatusCode.Unauthorized)
        {
        }
    }
}
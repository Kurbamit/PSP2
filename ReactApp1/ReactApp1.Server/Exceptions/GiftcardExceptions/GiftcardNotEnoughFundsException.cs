using System.Net;

namespace ReactApp1.Server.Exceptions.GiftCardExceptions
{
    public class GiftcardNotEnoughFundsException : BaseException
    {
        public GiftcardNotEnoughFundsException(decimal funds, decimal required)
            : base($"Not enough funds. Current funds: {funds}, required {required}")
        {
        }
    }
}
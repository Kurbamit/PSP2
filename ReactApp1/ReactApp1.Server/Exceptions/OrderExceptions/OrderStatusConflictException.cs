using System.Net;

namespace ReactApp1.Server.Exceptions.OrderExceptions
{
    public class OrderStatusConflictException : BaseException
    {
        public OrderStatusConflictException(string orderStatus)
            : base($"The operation cannot be performed because the order status is '{orderStatus}'", HttpStatusCode.Conflict)
        {
        }
    }
}
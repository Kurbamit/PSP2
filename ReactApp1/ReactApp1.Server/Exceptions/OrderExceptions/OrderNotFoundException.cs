using System.Net;

namespace ReactApp1.Server.Exceptions.OrderExceptions
{
    public class OrderNotFoundException : BaseException
    {
        public OrderNotFoundException(int orderId)
            : base($"Order (id = {orderId}) was not found", HttpStatusCode.NotFound)
        {
        }
    }
}
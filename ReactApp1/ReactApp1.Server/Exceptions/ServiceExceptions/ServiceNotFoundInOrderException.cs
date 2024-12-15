using System.Net;

namespace ReactApp1.Server.Exceptions.ServiceExceptions
{
    public class ServiceNotFoundInOrderException : BaseException
    {
        public ServiceNotFoundInOrderException(int orderId, int serviceId)
            : base($"Service (id = {serviceId}) was not found in order (id = {orderId})", HttpStatusCode.NotFound)
        {
        }
    }
}
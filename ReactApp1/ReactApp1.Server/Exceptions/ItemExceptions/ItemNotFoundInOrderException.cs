using System.Net;

namespace ReactApp1.Server.Exceptions.ItemExceptions
{
    public class ItemNotFoundInOrderException : BaseException
    {
        public ItemNotFoundInOrderException(int orderId, int itemId)
            : base($"Item (id = {itemId}) was not found in order (id = {orderId})", HttpStatusCode.NotFound)
        {
        }
    }
}
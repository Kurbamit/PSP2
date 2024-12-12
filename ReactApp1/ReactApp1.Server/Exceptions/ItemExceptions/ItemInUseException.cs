using System.Net;

namespace ReactApp1.Server.Exceptions.ItemExceptions
{
    public class ItemInUseException : BaseException
    {
        public ItemInUseException(int itemId)
            : base($"Item (id = {itemId}) is in use and cannot be deleted", HttpStatusCode.Conflict)
        {
        }
            
    }
}
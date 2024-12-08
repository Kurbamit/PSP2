using System.Net;

namespace ReactApp1.Server.Exceptions.ItemExceptions
{
    public class ItemNotFoundException : BaseException
    {
        public ItemNotFoundException(int itemId)
            : base($"Item (id = {itemId}) was not found", HttpStatusCode.NotFound)
        {
        }
    }
}


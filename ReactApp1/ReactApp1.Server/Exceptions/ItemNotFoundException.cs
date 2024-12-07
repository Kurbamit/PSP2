using System.Net;

namespace ReactApp1.Server.Exceptions
{
    public class ItemNotFoundException : BaseException
    {
        public ItemNotFoundException(int id)
            : base($"Item with id {id} not found", HttpStatusCode.NotFound)
        {
        }
    }
}


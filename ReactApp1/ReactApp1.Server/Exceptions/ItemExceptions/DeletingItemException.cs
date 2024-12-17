using System.Net;

namespace ReactApp1.Server.Exceptions.ItemExceptions
{
    public class DeletingItemException : BaseException
    {
        public DeletingItemException(int itemId)
            : base($"An error occurred while deleting the item {itemId} from the database.", HttpStatusCode.InternalServerError)
        {
        }
            
    }
}
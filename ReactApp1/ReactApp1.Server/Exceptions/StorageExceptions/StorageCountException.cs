using System.Net;

namespace ReactApp1.Server.Exceptions.StorageExceptions
{
    public class StorageCountException : BaseException
    {
        public StorageCountException(int itemId, int storageCount, int amount)
            : base($"Storage count of item (id = {itemId}) is {storageCount}. Cannot decrease by {-amount}.",
                HttpStatusCode.BadRequest)
        {
        }
    }
}
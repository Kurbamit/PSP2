using System.Net;

namespace ReactApp1.Server.Exceptions.ItemExceptions
{
    public class ItemIsBaseItemException : BaseException
    {
        public ItemIsBaseItemException(int itemId)
            : base($"Item with id = ({itemId}) can not be deleted, because it is a base item for some other item.", HttpStatusCode.Conflict)
        {
        }
            
    }
}
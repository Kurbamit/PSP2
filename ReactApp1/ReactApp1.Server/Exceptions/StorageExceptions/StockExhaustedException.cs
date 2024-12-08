using System.Net;

namespace ReactApp1.Server.Exceptions.StorageExceptions
{
    public class StockExhaustedException : BaseException
    {
        public StockExhaustedException(int itemId, int availableStock)
            : base($"The requested quantity of item (id = {itemId}) exceeds the available stock. Only {availableStock} items are in stock", HttpStatusCode.BadRequest)
        {
        }
    }
}
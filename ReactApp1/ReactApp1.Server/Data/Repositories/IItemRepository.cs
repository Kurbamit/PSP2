using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IItemRepository
    {
        Task<PaginatedItemsResponse<Item>> GetAllItemsAsync(int pageNumber, int pageSize);
        Task<Item?> GetItemByIdAsync(int itemId);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(int itemId);
    }
}


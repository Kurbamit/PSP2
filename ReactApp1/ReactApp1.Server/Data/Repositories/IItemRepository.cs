using ReactApp1.Server.Models;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllItemsAsync(int pageNumber, int pageSize);
        Task<Item?> GetItemByIdAsync(int itemId);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(int itemId);
    }
}


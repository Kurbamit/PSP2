using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IItemRepository
    {
        Task<PaginatedResult<Item>> GetAllItemsAsync(int pageNumber, int pageSize);
        Task<ItemModel?> GetItemByIdAsync(int itemId);
        Task<int> AddItemAsync(ItemModel item, int establishmentId, int userId);
        Task UpdateItemAsync(ItemModel item);
        Task AddStorageAsync(int itemId, int amount);
        Task<StorageModel?> GetItemStorageAsync(int itemId);
        Task DeleteItemAsync(int itemId);
        Task<ItemModel?> GetItemByIdFromFullOrderAsync(int itemId, int orderId);
    }
}


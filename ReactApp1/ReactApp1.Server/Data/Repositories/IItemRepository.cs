using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IItemRepository
    {
        Task<PaginatedItemsResponse<Item>> GetAllItemsAsync(int pageNumber, int pageSize);
        Task<ItemModel?> GetItemByIdAsync(int itemId);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(ItemModel item);
        Task DeleteItemAsync(int itemId);
    }
}


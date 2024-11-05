using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Services;

public interface IItemService
{
    Task<PaginatedItemsResponse<Item>> GetAllItems(int pageSize, int pageNumber);
    Task<Item?> GetItemById(int itemId);
    Task CreateNewItem(Item item);
    Task UpdateItem(Item item);
    Task DeleteItem(int itemId);
}
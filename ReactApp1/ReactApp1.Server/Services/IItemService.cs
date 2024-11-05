using ReactApp1.Server.Models;

namespace ReactApp1.Server.Services;

public interface IItemService
{
    Task<IEnumerable<Item>> GetAllItems(int pageSize, int pageNumber);
    Task<Item?> GetItemById(int itemId);
    Task CreateNewItem(Item item);
    Task UpdateItem(Item item);
    Task DeleteItem(int itemId);
}
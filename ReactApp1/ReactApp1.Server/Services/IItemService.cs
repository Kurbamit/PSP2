using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IItemService
{
    Task<PaginatedResult<Item>> GetAllItems(int pageSize, int pageNumber);
    Task<ItemModel?> GetItemById(int itemId);
    Task CreateNewItem(Item item, int? establishmentId);
    Task UpdateItem(ItemModel item);
    Task AddStorage(int itemId, int amount);
    Task DeleteItem(int itemId);
}
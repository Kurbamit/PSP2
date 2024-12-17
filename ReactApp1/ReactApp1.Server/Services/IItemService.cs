using System.Security.Principal;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services;

public interface IItemService
{
    Task<PaginatedResult<Item>> GetAllItems(int pageSize, int pageNumber, IPrincipal user);
    Task<ItemModel?> GetItemById(int itemId, IPrincipal user);
    Task<int> CreateNewItem(ItemModel item, int? establishmentId, int? userId);
    Task UpdateItem(ItemModel item);
    Task AddStorage(int itemId, int amount);
    Task DeleteItem(int itemId);
}
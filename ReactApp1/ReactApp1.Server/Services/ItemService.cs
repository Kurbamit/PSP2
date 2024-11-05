using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public Task<PaginatedItemsResponse<Item>> GetAllItems(int pageSize, int pageNumber)
        {
            return _itemRepository.GetAllItemsAsync(pageSize, pageNumber);
        }

        public Task<Item?> GetItemById(int itemId)
        {
            return _itemRepository.GetItemByIdAsync(itemId);
        }

        public Task CreateNewItem(Item item)
        {
            return _itemRepository.AddItemAsync(item);
        }

        public Task UpdateItem(Item item)
        {
            return _itemRepository.UpdateItemAsync(item);
        }

        public Task DeleteItem(int itemId)
        {
            return _itemRepository.DeleteItemAsync(itemId);
        }
    }
}


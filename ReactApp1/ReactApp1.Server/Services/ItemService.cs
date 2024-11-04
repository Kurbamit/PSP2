using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public Task<IEnumerable<Item>> GetAllItems(int pageSize, int pageNumber)
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


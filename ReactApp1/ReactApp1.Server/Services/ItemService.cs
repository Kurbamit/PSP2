using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public Task<PaginatedResult<Item>> GetAllItems(int pageSize, int pageNumber)
        {
            return _itemRepository.GetAllItemsAsync(pageSize, pageNumber);
        }

        public Task<ItemModel?> GetItemById(int itemId)
        {
            return _itemRepository.GetItemByIdAsync(itemId);
        }

        public Task CreateNewItem(Item item, int? establishmentId)
        {
            return _itemRepository.AddItemAsync(item, establishmentId);
        }

        public Task UpdateItem(ItemModel item)
        {
            return _itemRepository.UpdateItemAsync(item);
        }
        
        public Task AddStorage(int itemId, int amount)
        {
            return _itemRepository.AddStorageAsync(itemId, amount);
        }

        public Task DeleteItem(int itemId)
        {
            return _itemRepository.DeleteItemAsync(itemId);
        }
    }
}


using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemService> _logger;

        public ItemService(IItemRepository itemRepository, ILogger<ItemService> logger)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        public Task<PaginatedResult<Item>> GetAllItems(int pageSize, int pageNumber)
        {
            return _itemRepository.GetAllItemsAsync(pageSize, pageNumber);
        }

        public Task<ItemModel?> GetItemById(int itemId)
        {
            return _itemRepository.GetItemByIdAsync(itemId);
        }

        public Task<int> CreateNewItem(ItemModel item, int? establishmentId, int? userId)
        {
            if (!userId.HasValue || !establishmentId.HasValue)
            {
                _logger.LogError("Failed to create item: invalid or expired access token");
                throw new UnauthorizedAccessException("Operation failed: Invalid or expired access token");
            }
            
            return _itemRepository.AddItemAsync(item, establishmentId.Value, userId.Value);
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


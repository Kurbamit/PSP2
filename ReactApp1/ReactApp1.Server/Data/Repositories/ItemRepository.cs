using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Exceptions;
using ReactApp1.Server.Exceptions.ItemExceptions;
using ReactApp1.Server.Exceptions.StorageExceptions;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<PaginatedResult<Item>> GetAllItemsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Set<Item>().CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            var items = await _context.Set<Item>()
                .OrderBy(item => item.ItemId)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();
            
            return new PaginatedResult<Item>
            {
                Items = items,
                TotalPages = totalPages,
                TotalItems = totalItems,
                CurrentPage = pageNumber
            };
        }

        public async Task<ItemModel?> GetItemByIdAsync(int itemId)
        {
            var item = await _context.Items
                .Where(f => f.ItemId == itemId)
                .Select(f => new ItemModel()
                {
                    ItemId = f.ItemId,
                    Name = f.Name,
                    Cost = f.Cost,
                    AlcoholicBeverage = f.AlcoholicBeverage,
                    ReceiveTime = f.ReceiveTime,
                    BaseItemId = f.BaseItemId,
                    Storage = f.Storage == null ? null : f.Storage.Count
                }).FirstOrDefaultAsync();
            return item;
        }

        /// <summary>
        /// Method to get historical data of an item from a full order.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<ItemModel?> GetItemByIdFromFullOrderAsync(int itemId, int orderId)
        {
            var item = await _context.FullOrders
                .Where(f => f.ItemId == itemId)
                .Where(f => f.OrderId == orderId)
                .Select(f => new ItemModel()
                {
                    ItemId = f.ItemId,
                    Name = f.Name,
                    Cost = f.Cost,
                    AlcoholicBeverage = f.AlcoholicBeverage,
                    ReceiveTime = f.ReceiveTime,
                }).FirstOrDefaultAsync();

            var storage = await _context.Storages
                .Where(f => f.ItemId == itemId)
                .Select(f => f.Count)
                .FirstOrDefaultAsync();

            if (item != null)
            {
                item.Storage = storage;
            }

            return item;
        }
        
        public async Task<int> AddItemAsync(ItemModel item, int establishmentId, int userId)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                var newItem = new Item()
                {
                    Name = item.Name,
                    Cost = item.Cost,
                    AlcoholicBeverage = item.AlcoholicBeverage,
                    ReceiveTime = DateTime.UtcNow,
                    BaseItemId = item.BaseItemId,
                    EstablishmentId = establishmentId,
                    CreatedByEmployeeId = userId
                };
                
                await _context.Items.AddAsync(newItem);
                await _context.SaveChangesAsync();
                
                var storage = new Storage()
                {
                    ItemId = newItem.ItemId,
                    Count = 0,
                    EstablishmentId = establishmentId
                };
                await _context.Set<Storage>().AddAsync(storage);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return newItem.ItemId;
            }   
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new item to the database.", e);
            }
        }
        
        public async Task UpdateItemAsync(ItemModel item)
        {
            try
            {
                var existingItem = await _context.Set<Item>()
                    .Include(i => i.Storage)
                    .FirstOrDefaultAsync(i => i.ItemId == item.ItemId);
                
                
                if (existingItem == null)
                {
                    throw new KeyNotFoundException($"Item with ID {item.ItemId} not found.");
                }
                
                item.MapUpdate(existingItem);

                _context.Set<Item>().Update(existingItem);
                await _context.SaveChangesAsync();
            }   
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating the item: {item.ItemId}.", e);
            }
        }

        public async Task AddStorageAsync(int itemId, int amount)
        {
            try
            {
                var storage = await _context.Storages
                    .Where(f => f.ItemId == itemId)
                    .FirstOrDefaultAsync();

                if (storage == null)
                {
                    throw new KeyNotFoundException($"Storage with item ID {itemId} not found.");
                }
                
                if (storage.Count + amount < 0)
                {
                    throw new StorageCountException(itemId, storage.Count, amount);
                }

                storage.Count += amount;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating storage with the item: {itemId}.", e);
            }
        }
        
        public async Task<StorageModel?> GetItemStorageAsync(int itemId)
        {

            var storage = await _context.Storages
                .Where(f => f.ItemId == itemId)
                .FirstOrDefaultAsync();

            return storage != null
                ? new StorageModel(storage)
                : null;
        }

        
        public async Task DeleteItemAsync(int itemId)
        {
            try
            {
                var isItemInUse = await _context.FullOrders
                    .AnyAsync(f => f.ItemId == itemId);

                if (isItemInUse)
                {
                    throw new ItemInUseException(itemId);
                }
                
                _context.Set<Item>().Remove(new Item
                {
                    ItemId = itemId 
                });
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the item {itemId} from the database.", e);
            }
        }
    }
}


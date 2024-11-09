using Microsoft.EntityFrameworkCore;
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
                    Tax = f.Tax,
                    AlcoholicBeverage = f.AlcoholicBeverage,
                    ReceiveTime = f.ReceiveTime,
                    Storage = f.Storage == null ? null : f.Storage.Count
                }).FirstOrDefaultAsync();
            return item;
        }
        
        public async Task AddItemAsync(Item item, int? establishmentId)
        {
            if (!establishmentId.HasValue)
            {
                throw new ArgumentNullException(nameof(establishmentId));
            }
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                await _context.Set<Item>().AddAsync(item);
                await _context.SaveChangesAsync();
                
                var storage = new Storage()
                {
                    ItemId = item.ItemId,
                    Count = 0,
                    EstablishmentId = establishmentId.Value
                };
                await _context.Set<Storage>().AddAsync(storage);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
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

                storage.Count += amount;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating storage with the item: {itemId}.", e);
            }
        }
        
        public async Task DeleteItemAsync(int itemId)
        {
            try
            {
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


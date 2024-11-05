using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<PaginatedItemsResponse<Item>> GetAllItemsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Set<Item>().CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            var items = await _context.Set<Item>()
                .OrderBy(item => item.ItemId)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();
            
            return new PaginatedItemsResponse<Item>
            {
                Items = items,
                TotalPages = totalPages,
                TotalItems = totalItems,
                CurrentPage = pageNumber
            };
        }

        public async Task<Item?> GetItemByIdAsync(int itemId)
        {
            return await _context.Set<Item>().FindAsync(itemId);
        }
        
        public async Task AddItemAsync(Item item)
        {
            try
            {
                await _context.Set<Item>().AddAsync(item);
                await _context.SaveChangesAsync();

            }   
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding new item to the database.", e);
            }
        }
        
        public async Task UpdateItemAsync(Item item)
        {
            try
            {
                _context.Set<Item>().Update(item);
                await _context.SaveChangesAsync();

            }   
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating the item: {item.ItemId}.", e);
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


using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ItemController : ControllerBase
    { 
        private readonly IItemService _itemService;
        
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        
        [HttpGet("items")]
        public async Task<IActionResult> GetOrderItems(int pageNumber, int pageSize)
        {
            var items = await _itemService.GetAllItems(pageNumber, pageSize);

            return Ok(items);
        }
        
        [HttpGet("items/{itemId}")]
        public async Task<IActionResult> GetOrderItems(int itemId)
        {
            var item = await _itemService.GetItemById(itemId);

            return Ok(item);
        }
        
        [HttpPost("items")]
        public async Task<IActionResult> CreateItem([FromBody] Item item)
        {
            await _itemService.CreateNewItem(item);
            
            return Ok();
        }
        
        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItem([FromBody] Item item)
        {
            await _itemService.UpdateItem(item);
            
            return Ok();
        }
        
        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            await _itemService.DeleteItem(itemId);
            
            return Ok();
        }
    }
}

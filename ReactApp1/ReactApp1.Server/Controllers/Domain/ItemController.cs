using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;
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
        
        #region Endpoints
        [Authorize]
        [HttpGet("items")]
        public async Task<IActionResult> GetItems(int pageNumber, int pageSize)
        {
            var items = await _itemService.GetAllItems(pageNumber, pageSize, User);

            return Ok(items);
        }
        
        [Authorize]
        [HttpGet("items/{itemId}")]
        public async Task<IActionResult> GetItems(int itemId)
        {
            var item = await _itemService.GetItemById(itemId, User);

            return Ok(item);
        }
        
        [Authorize]
        [HttpPost("items")]
        public async Task<IActionResult> CreateItem([FromBody] ItemModel item)
        {
            var establishmentId = User.GetUserEstablishmentId();
            var userId = User.GetUserId();
            var newItemId = await _itemService.CreateNewItem(item, establishmentId, userId);
            
            return Ok(newItemId);
        }
        
        [Authorize]
        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItem([FromBody] ItemModel item)
        {
            await _itemService.UpdateItem(item);
            
            return Ok();
        }

        [Authorize]
        [HttpPut("items/{itemId}/add-storage")]
        public async Task<IActionResult> AddStorage(int itemId, [FromQuery] int amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than 0");
            }
            await _itemService.AddStorage(itemId, amount);

            return Ok();
        }
        
        [Authorize]
        [HttpPut("items/{itemId}/deduct-storage")]
        public async Task<IActionResult> DeductStorage(int itemId, [FromQuery] int amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than 0");
            }
            await _itemService.AddStorage(itemId, -amount);

            return Ok();
        }
        
        [Authorize]
        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            await _itemService.DeleteItem(itemId);
            
            return NoContent();
        }
        #endregion Endpoints
    }
}

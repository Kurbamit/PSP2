using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/")]
    public class GiftCardController : ControllerBase
    { 
        private readonly IGiftCardService _giftCardService;
        
        public GiftCardController(IGiftCardService giftCardService)
        {
            _giftCardService = giftCardService;
        }
        
        #region Endpoints
        [HttpGet("giftCards")]
        public async Task<IActionResult> GetGiftCards(int pageNumber, int pageSize)
        {
            var giftCards = await _giftCardService.GetAllGiftCards(pageNumber, pageSize);

            return Ok(giftCards);
        }
        
        [HttpGet("giftCards/{giftCardId}")]
        public async Task<IActionResult> GetGiftCards(int giftCardId)
        {
            var giftCard = await _giftCardService.GetGiftCardById(giftCardId);

            return Ok(giftCard);
        }
        
        [HttpPost("giftCards")]
        public async Task<IActionResult> CreateGiftCard([FromBody] GiftCardModel giftCard)
        {
            
            await _giftCardService.CreateNewGiftCard(giftCard);
            
            return Ok(giftCard.GiftCardId);
        }
        
        [HttpPut("giftCards/{giftCardId}")]
        public async Task<IActionResult> UpdateGiftCard([FromBody] GiftCardModel giftCard)
        {
            await _giftCardService.UpdateGiftCard(giftCard);
            
            return Ok();
        }
        
        [HttpDelete("giftCards/{giftCardId}")]
        public async Task<IActionResult> DeleteGiftCard(int giftCardId)
        {
            await _giftCardService.DeleteGiftCard(giftCardId);
            
            return NoContent();
        }
        #endregion Endpoints
    }
}

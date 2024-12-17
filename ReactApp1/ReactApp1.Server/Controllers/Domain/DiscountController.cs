using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;

namespace ReactApp1.Server.Controllers.Domain
{
    [ApiController]
    [Route("api/")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [Authorize]
        [HttpGet("discounts")]
        public async Task<IActionResult> GetDiscounts([FromQuery] int pageNumber, int pageSize)
        {
            var discounts = await _discountService.GetAllDiscounts(pageNumber, pageSize, User);

            return Ok(discounts);
        }

        [Authorize]
        [HttpGet("discounts/{discountId}")]
        public async Task<IActionResult> GetDiscount(int discountId)
        {
            var discount = await _discountService.GetDiscountByIdAsync(discountId, User);

            return Ok(discount);
        }

        [Authorize]
        [HttpPost("discounts")]
        public async Task<IActionResult> CreateDiscount([FromBody] DiscountModelForAPI discount)
        {
            var newDiscount = await _discountService.CreateDiscount(discount, User);

            return Ok(newDiscount);
        }
        
        [Authorize]
        [HttpPut("discounts")]
        public async Task<IActionResult> UpdateDiscount([FromBody] DiscountModelForAPI discount)
        {
            await _discountService.UpdateDiscount(discount);

            return Ok();
        }
        
        [Authorize]
        [HttpDelete("discounts/{discountId}")]
        public async Task<IActionResult> DeleteDiscount(int discountId)
        {
            await _discountService.DeleteDiscount(discountId, User);

            return Ok();
        }
    }
}


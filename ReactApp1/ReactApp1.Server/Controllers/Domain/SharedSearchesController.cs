using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Services;

namespace ReactApp1.Server.Controllers.Domain
{
    [ApiController]
    [Route("api/")]
    public class SharedSearchesController : ControllerBase
    {
        private readonly ISharedSearchesService _sharedSearchesService;
        
        public SharedSearchesController(ISharedSearchesService sharedSearchesService)
        {
            _sharedSearchesService = sharedSearchesService;
        }
        
        [HttpGet("AllItems")]
        public async Task<IActionResult> AllItems([FromQuery] string? search)
        {
            var establishmentId = User.GetUserEstablishmentId();
            
            var result = await _sharedSearchesService.GetAllItems(establishmentId, search);
            
            return Ok(result);
        }

        [HttpGet("AllServices")]
        public async Task<IActionResult> AllServices([FromQuery] string? search)
        {
            var establishmentId = User.GetUserEstablishmentId();

            var result = await _sharedSearchesService.GetAllServices(establishmentId, search);
            
            return Ok(result);
        }
        
        [HttpGet("AllDiscounts")]
        public async Task<IActionResult> AllDiscounts([FromQuery] string? search)
        {
            var establishmentId = User.GetUserEstablishmentId();

            var result = await _sharedSearchesService.GetAllDiscounts(establishmentId, search);

            return Ok(result);
        }
        [HttpGet("AllTaxes")]
        public async Task<IActionResult> AllTaxes([FromQuery] string? search)
        {
            var result = await _sharedSearchesService.GetAllTaxes(search);

            return Ok(result);
        }

        [HttpGet("AllBaseItems")]
        public async Task<IActionResult> AllBaseItems([FromQuery] string? search)
        {
            var establishmentId = User.GetUserEstablishmentId();

            var result = await _sharedSearchesService.GetAllBaseItems(establishmentId, search);

            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
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
        
        [Authorize]
        [HttpGet("AllItems")]
        public async Task<IActionResult> AllItems([FromQuery] string? search)
        {
            var establishmentId = User.GetUserEstablishmentId();
            
            var result = await _sharedSearchesService.GetAllItems(establishmentId, search, User);
            
            return Ok(result);
        }

        [Authorize]
        [HttpGet("AllServices")]
        public async Task<IActionResult> AllServices([FromQuery] string? search)
        {
            var establishmentId = User.GetUserEstablishmentId();

            var result = await _sharedSearchesService.GetAllServices(establishmentId, search, User);
            
            return Ok(result);
        }
        
        [Authorize]
        [HttpGet("AllDiscounts")]
        public async Task<IActionResult> AllDiscounts([FromQuery] string? search)
        {
            var establishmentId = User.GetUserEstablishmentId();

            var result = await _sharedSearchesService.GetAllDiscounts(establishmentId, search, User);

            return Ok(result);
        }
    }
}

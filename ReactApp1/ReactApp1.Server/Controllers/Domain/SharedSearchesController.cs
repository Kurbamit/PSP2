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
    }
}

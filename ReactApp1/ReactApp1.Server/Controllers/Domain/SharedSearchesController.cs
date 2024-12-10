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
            // TODO: Prideti i "Items" lenta FK EstablishmentId, kad pagal tai butu galima atrinkti tik tuos items, kurie priklauso tam establishmentui
            var establishmentId = User.GetUserEstablishmentId();
            if (establishmentId == null)
            {
                return BadRequest("Establishment not found.");
            }
            
            var result = await _sharedSearchesService.GetAllItems(establishmentId.Value, search);
            
            return Ok(result);
        }
    }
}

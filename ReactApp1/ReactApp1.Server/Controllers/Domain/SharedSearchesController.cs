using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Data;
using ReactApp1.Server.Models.Models.Base;

namespace ReactApp1.Server.Controllers.Domain
{
    [ApiController]
    [Route("api/")]
    public class SharedSearchesController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public SharedSearchesController(AppDbContext context)
        {
            _context = context;
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

            var result = await _context.Items
                .WhereIf(!string.IsNullOrWhiteSpace(search), f => f.Name.ToLower().Contains(search.ToLower()))
                .Select(f => new SharedItem()
                {
                    Id = f.ItemId,
                    Name = f.Name == null ? "None" : f.Name
                }).ToListAsync();
            
            return Ok(result);
        }
    }
}

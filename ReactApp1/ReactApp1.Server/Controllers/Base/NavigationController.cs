using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ReactApp1.Server.Data;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Services;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/")]
    public class NavigationController : ControllerBase
    {
        private readonly ILogger<NavigationController> _logger;
        private readonly AppDbContext _context;

        public NavigationController(ILogger<NavigationController> logger, IOptions<JwtSettings> jwtSettings, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("navigation")]
        public async Task<IActionResult> Navigation()
        {
            var userRole = User.GetUserTitle();
            
            // if (!userRole.HasValue)
            // {
            //     return BadRequest("User role not found.");
            // }

            var navigation = SiteLinkManager.GetNavigationBasedOnRole(userRole);

            return Ok(navigation);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ReactApp1.Server.Data;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly JwtSettings _jwtSettings;
        private readonly AppDbContext _context;

        public LoginController(ILogger<RegisterController> logger, IOptions<JwtSettings> jwtSettings, AppDbContext context)
        {
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }
        
        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("Username is required.");
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Password is required.");
            }

            var user = await _context.Employees
                .Where(f => f.Email == request.Email)
                .FirstOrDefaultAsync();
            
            if (user == null || !VerifyPassword(user.PasswordHash, request.Password))
            {
                return BadRequest("Invalid username or password.");
            }
            
            var token = GenerateJwtToken(request.Email);
            
            Response.Cookies.Append("authToken", token, new CookieOptions
            {
                HttpOnly = true, // Prevent client-side access
                Secure = true, // Set to true if using HTTPS
                SameSite = SameSiteMode.None, // Required for cross-origin requests
                Expires = DateTime.UtcNow.AddDays(1) // Cookie expiration
            });
            
            return Ok(new { message = "Login successful", token });
        }
        
        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        private bool VerifyPassword (string? passwordHash, string? password)
        {
            if (passwordHash == null || password == null)
            {
                return false;
            }
            
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Data;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly JwtSettings _jwtSettings;
        private readonly AppDbContext _context;

        public RegisterController(ILogger<RegisterController> logger, IOptions<JwtSettings> jwtSettings, AppDbContext context)
        {
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }

        [HttpPost(Name = "Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return BadRequest("Username is required.");
            }
            if (!IsValidEmail(request.Email))
            {
                return BadRequest("Email is not valid.");
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Password is required.");
            }
            
            // Check if the email already exists
            var existingUser = await _context.Employees
                .Where(e => e.Email == request.Email)
                .FirstOrDefaultAsync();
            
            if (existingUser != null)
            {
                return BadRequest("An account with this email already exists.");
            }
            
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            
            var newEmployee = new Employee
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = passwordHash,
                EstablishmentId = request.EstablishmentId,
                AddressId = request.AddressId,
                Phone = request.Phone
            };
            
            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Registration successful" });
        }
        
        private bool IsValidEmail (string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith(".")) {
                return false;
            }
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch{
                return false;
            }
        }
    }
}
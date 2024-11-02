using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "Register")]
        public IActionResult Register([FromBody] RegisterRequest request)
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
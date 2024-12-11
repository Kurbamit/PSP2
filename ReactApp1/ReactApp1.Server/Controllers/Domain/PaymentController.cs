using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;
using Stripe;

namespace ReactApp1.Server.Controllers.Domain
{
    [ApiController]
    [Route("api/")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        
        #region Endpoints

        [HttpPost("payments/createIntent/{amount}/{currency}")]
        public async Task<IActionResult> CreatePaymentIntent(decimal amount, string currency)
        {
            var paymentIntent = await _paymentService.CreatePaymentIntent(amount, currency);

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
        #endregion Endpoints
    }
}
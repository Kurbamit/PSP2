using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;

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

        [HttpGet("payments")]
        public async Task<IActionResult> GetPayments(int pageNumber, int pageSize)
        {
            var payments = await _paymentService.GetAllPayments(pageNumber, pageSize);

            return Ok(payments);
        }

        [HttpGet("payments/{paymentId}")]
        public async Task<IActionResult> GetPayment(int paymentId)
        {
            var payment = await _paymentService.GetPaymentById(paymentId);

            return Ok(payment);
        }

        [HttpGet("payments/order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrderId(int orderId)
        {
            var payments = await _paymentService.GetPaymentsByOrderId(orderId);

            return Ok(payments);
        }

        [HttpPost("payments")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentModel payment)
        {
            await _paymentService.CreateNewPayment(payment);
            
            return Ok(payment.PaymentId);
        }
        
        [HttpPut("payments/{paymentId}")]
        public async Task<IActionResult> UpdatePayment([FromBody] PaymentModel payment)
        {
            await _paymentService.UpdatePayment(payment);
            
            return Ok();
        }
        
        [HttpDelete("payments/{paymentId}")]
        public async Task<IActionResult> DeletePayment(int paymentId)
        {
            await _paymentService.DeletePayment(paymentId);
            
            return NoContent();
        }
        
        #endregion Endpoints
    }
}
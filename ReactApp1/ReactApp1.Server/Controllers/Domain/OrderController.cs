using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models.Models.Domain;
using ReactApp1.Server.Services;

namespace ReactApp1.Server.Controllers.Domain
{
    [ApiController]
    [Route("api/")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders([FromQuery] int pageNumber, int pageSize)
        {
            var orders = await _orderService.GetAllOrders(pageNumber, pageSize);
            
            return Ok(orders);
        }
        
        [HttpGet("orders/{orderId}")]
        public async Task<IActionResult> GetOrders(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            
            return Ok(order);
        }
        
        [HttpPost("orders")]
        public async Task<IActionResult> OpenOrder()
        {
            var userId = User.GetUserId();
            var establishmentId = User.GetUserEstablishmentId();
            var emptyOrder = await _orderService.OpenOrder(userId, establishmentId);
            
            return Ok(emptyOrder);
        }
        
        [HttpPut("orders/{orderId}/items")]
        public async Task<IActionResult> AddItemToOrder([FromBody] FullOrderModel order)
        {
            var userId = User.GetUserId();
            
            await _orderService.AddItemToOrder(order, userId);

            return Ok();
        }
        
        // This endpoint can also be used to apply a discount
        [HttpPut("orders/{orderId}")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderModel order)
        {
            await _orderService.UpdateOrder(order);

            return Ok();
        }
        
        [HttpPut("orders/{orderId}/close")]
        public async Task<IActionResult> CloseOrder(int orderId)
        {
            await _orderService.CloseOrder(orderId);

            return Ok();
        }

        [HttpPut("orders/{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            await _orderService.CancelOrder(orderId);

            return Ok();
        }

        [HttpPut("orders/{orderId}/pay")]
        public async Task<IActionResult> PayOrder([FromBody] PaymentModel payment)
        {
            await _orderService.PayOrder(payment);

            return Ok();
        }

        [HttpPut("orders/{orderId}/refund")]
        public async Task<IActionResult> RefundOrder(int orderId)
        {
            await _orderService.RefundOrder(orderId);

            return Ok();
        }

        [HttpDelete("orders/{orderId}/items")]
        public async Task<IActionResult> RemoveItemFromOrder([FromBody] FullOrderModel order)
        {
            await _orderService.RemoveItemFromOrder(order);

            return Ok();
        }

        [HttpGet("orders/{orderId}/download")]
        public async Task<IActionResult> DownloadReceipt(int orderId)
        {
            var receipt = await _orderService.DownloadReceipt(orderId);

            return File(receipt, "text/plain", $"Receipt_Order{orderId}.txt");
        }
    }
}


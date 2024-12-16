using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders([FromQuery] int pageNumber, int pageSize)
        {
            var orders = await _orderService.GetAllOrders(pageNumber, pageSize, User);
            
            return Ok(orders);
        }
        
        [Authorize]
        [HttpGet("orders/{orderId}")]
        public async Task<IActionResult> GetOrders(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId, User);
            
            return Ok(order);
        }
        
        [Authorize]
        [HttpPost("orders")]
        public async Task<IActionResult> OpenOrder()
        {
            var userId = User.GetUserId();
            var establishmentId = User.GetUserEstablishmentId();
            var emptyOrder = await _orderService.OpenOrder(userId, establishmentId);
            
            return Ok(emptyOrder);
        }
        
        [Authorize]
        [HttpPut("orders/{orderId}/items")]
        public async Task<IActionResult> AddItemToOrder([FromBody] FullOrderModel order)
        {
            var userId = User.GetUserId();
            
            await _orderService.AddItemToOrder(order, userId, User);

            return Ok();
        }

        [Authorize]
        [HttpPut("orders/{orderId}/services")]
        public async Task<IActionResult> AddServiceToOrder([FromBody] FullOrderServiceModel order)
        {
            var userId = User.GetUserId();

            await _orderService.AddServiceToOrder(order, userId, User);

            return Ok();
        }
        
        // This endpoint can also be used to apply a discount
        [Authorize]
        [HttpPut("orders/{orderId}")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderModel order)
        {
            await _orderService.UpdateOrder(order, User);

            return Ok();
        }
        
        [Authorize]
        [HttpPut("orders/{orderId}/close")]
        public async Task<IActionResult> CloseOrder(int orderId)
        {
            await _orderService.CloseOrder(orderId, User);

            return Ok();
        }

        [Authorize]
        [HttpPut("orders/{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            await _orderService.CancelOrder(orderId, User);

            return Ok();
        }

        [Authorize]
        [HttpPut("orders/{orderId}/pay")]
        public async Task<IActionResult> PayOrder([FromBody] PaymentModel payment)
        {
            await _orderService.PayOrder(payment, User);

            return Ok();
        }

        [HttpPut("orders/{orderId}/refund")]
        public async Task<IActionResult> RefundOrder(int orderId)
        {
            await _orderService.RefundOrder(orderId, User);

            return Ok();
        }

        [HttpPut("orders/{orderId}/tip")]
        public async Task<IActionResult> TipOrder([FromBody] TipModel tip)
        {
            await _orderService.TipOrder(tip, User);

            return Ok();
        }

        [Authorize]
        [HttpPut("orders/{orderId}/discount")]
        public async Task<IActionResult> DiscountOrder([FromBody] DiscountModel discount)
        {
            await _orderService.DiscountOrder(discount, User);

            return Ok();
        }

        [Authorize]
        [HttpDelete("orders/{orderId}/items")]
        public async Task<IActionResult> RemoveItemFromOrder([FromBody] FullOrderModel order)
        {
            await _orderService.RemoveItemFromOrder(order, User);

            return Ok();
        }

        [Authorize]
        [HttpDelete("orders/{orderId}/services")]
        public async Task<IActionResult> RemoveServiceFromOrder([FromBody] FullOrderServiceModel order)
        {
            await _orderService.RemoveServiceFromOrder(order, User);

            return Ok();
        }

        [Authorize]
        [HttpGet("orders/{orderId}/download")]
        public async Task<IActionResult> DownloadReceipt(int orderId)
        {
            var receipt = await _orderService.DownloadReceipt(orderId, User);

            return File(receipt, "text/plain", $"Receipt_Order{orderId}.txt");
        }
    }
}


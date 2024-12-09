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
            await _orderService.AddItemToOrder(order);

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
        
        [HttpDelete("orders/{orderId}/items")]
        public async Task<IActionResult> RemoveItemFromOrder([FromBody] FullOrderModel order)
        {
            await _orderService.RemoveItemFromOrder(order);

            return Ok();
        }
    }
}


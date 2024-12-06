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
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber, int pageSize)
        {
            var orders = await _orderService.GetAllOrders(pageNumber, pageSize);
            
            return Ok(orders);
        }
        
        [HttpPost("orders")]
        public async Task<IActionResult> OpenOrder()
        {
            var userId = User.GetUserId();
            var emptyOrder = await _orderService.OpenOrder(userId);
            
            return Ok(emptyOrder);
        }
        
        [HttpPut("orders/{orderId}/items")]
        public async Task<IActionResult> AddItemToOrder([FromBody] FullOrderModel order)
        {
            await _orderService.AddItemToOrder(order);

            return Ok();
        }
    }
}


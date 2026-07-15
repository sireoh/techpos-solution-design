using Microsoft.AspNetCore.Mvc;
using Onion.Application.Services;

namespace Onion.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _ordersService;

        public OrdersController(OrderService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(string customerName, decimal totalAmount)
        {
            var orderId = await _ordersService.CreateOrderAsync(customerName, totalAmount);
            return Ok(orderId);
        }
    }
}

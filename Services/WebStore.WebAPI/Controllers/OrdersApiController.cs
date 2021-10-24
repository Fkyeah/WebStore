using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebStore.Interfaces.WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetUserOrders(string userName)
        {
            var result = await _orderService.GetUserOrders(userName);
            return Ok(result.ToDTO());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderById(id);
            if (result is null)
                return NotFound();
            return Ok(result.ToDTO());
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> CreateOrder(string userName, [FromBody] CreateOrderDTO orderDTO)
        {
            var result = await _orderService.CreateOrder(userName, orderDTO.Items.ToCartView(), orderDTO.Order);
            return Ok(result.ToDTO());
        }
    }
}

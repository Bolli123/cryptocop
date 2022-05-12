using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        [Route("")]
        public IActionResult GetAllOrders()
        {
            return Ok(_orderService.GetOrders(User.Identity.Name));
        }
        [HttpPost]
        [Route("")]
        public IActionResult AddOrder([FromBody] OrderInputModel order)
        {
            if (!ModelState.IsValid ) { return BadRequest();}
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "name").Value, out var email);
            _orderService.CreateNewOrder(User.Identity.Name, order);
            return StatusCode(201);
        }
    }
}
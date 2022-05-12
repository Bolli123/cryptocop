using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllPaymentCards()
        {
            var claims = User.Claims.Select(c => new {
                Type = c.Type,
                Value = c.Value
            });
            return Ok(_paymentService.GetStoredPaymentCards(User.Identity.Name));
        }
        [HttpPost]
        [Route("")]
        public IActionResult AddPaymentCard([FromBody] PaymentCardInputModel card)
        {
            if (!ModelState.IsValid ) { return BadRequest();}
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "name").Value, out var email);
            _paymentService.AddPaymentCard(User.Identity.Name, card);
            return StatusCode(201);
        }
    }
}
using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAddresses()
        {
            var claims = User.Claims.Select(c => new {
                Type = c.Type,
                Value = c.Value
            });
            return Ok(_addressService.GetAllAddresses(User.Identity.Name));
        }
        [HttpPost]
        [Route("")]
        public IActionResult AddAddress([FromBody] AddressInputModel address)
        {
            if (!ModelState.IsValid ) { return BadRequest();}
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "name").Value, out var email);
            _addressService.AddAddress(User.Identity.Name, address);
            return StatusCode(201);
        }
        [HttpDelete]
        [Route("{addressId}")]
        public IActionResult DeleteAddress(int addressId)
        {
            _addressService.DeleteAddress(User.Identity.Name, addressId);
            return Ok();
        }
    }
}
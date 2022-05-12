using System;
using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllShoppingCartItems()
        {
            return Ok(_shoppingCartService.GetCartItems(User.Identity.Name));
        }
        [HttpPost]
        [Route("")]
        public IActionResult AddCartItem([FromBody] ShoppingCartItemInputModel cartItem)
        {
            if (!ModelState.IsValid ) { return BadRequest();}
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "name").Value, out var email);
            _shoppingCartService.AddCartItem(User.Identity.Name, cartItem);
            return StatusCode(201);
        }
        [HttpDelete]
        [Route("")]
        public IActionResult ClearCart()
        {
            _shoppingCartService.ClearCart(User.Identity.Name);
            return NoContent();
        }
        [HttpDelete]
        [Route("{cartId}")]
        public IActionResult DeleteCartItem(int cartId)
        {
            _shoppingCartService.RemoveCartItem(User.Identity.Name, cartId);
            return NoContent();
        }
        [HttpPatch]
        [Route("{cartId}")]
        public IActionResult UpdateCartItemQuantity(int cartId, [FromBody] ShoppingCartItemInputModel cartItem)
        {
            var quant = cartItem;
            if (quant == null || quant.Quantity == null) {return BadRequest();}
            _shoppingCartService.UpdateCartItemQuantity(User.Identity.Name, cartId, (float) quant.Quantity);
            return NoContent();
        }
    }
}
using System;
using System.Linq;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult RegisterAccount([FromBody] RegisterInputModel account)
        {
            if (!ModelState.IsValid ) { return BadRequest();}
            var user = _accountService.CreateUser(account);
            if (user == null) { throw new ConflictException();}
            return Ok(_tokenService.GenerateJwtToken(user));
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn([FromBody] LoginInputModel account)
        {
            if (!ModelState.IsValid ) { return BadRequest();}
            var user = _accountService.AuthenticateUser(account);
            if (user == null) { return Unauthorized();}
            return Ok(_tokenService.GenerateJwtToken(user));
        }
        [HttpGet]
        [Route("signout")] 
        public IActionResult SignOut()
        {
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "tokenId").Value, out var tokenId);
            _accountService.Logout(tokenId);
            return NoContent();
        }
    }
}
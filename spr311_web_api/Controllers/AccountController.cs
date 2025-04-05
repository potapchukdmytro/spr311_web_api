﻿using Microsoft.AspNetCore.Mvc;
using spr311_web_api.BLL.Dtos.Account;
using spr311_web_api.BLL.Services.Account;
using spr311_web_api.BLL.Validators.Account;

namespace spr311_web_api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly RegisterValidator _registerValidator;

        public AccountController(IAccountService accountService, RegisterValidator registerValidator)
        {
            _accountService = accountService;
            _registerValidator = registerValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto dto)
        {
            var response = await _accountService.LoginAsync(dto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto dto)
        {
            var validResult = await _registerValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
            {
                return BadRequest(validResult.Errors);
            }

            var user = await _accountService.RegisterAsync(dto);

            if(user == null)
            {
                return BadRequest("Register error");
            }

            return Ok(user);
        }

        [HttpGet("confirmEmail")]
        public IActionResult ConfirmEmail(string? userId, string? token)
        {
            // потрібно написати код підтвердження пошти
            return Redirect("http://localhost:3000");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using spr311_web_api.BLL.Dtos.Account;
using spr311_web_api.BLL.Services.Account;

namespace spr311_web_api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto dto)
        {
            // тут має бути валідація

            var user = await _accountService.RegisterAsync(dto);

            if(user == null)
            {
                return BadRequest("Register error");
            }

            return Ok(user);
        }
    }
}

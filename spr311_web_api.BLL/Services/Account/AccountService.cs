using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using spr311_web_api.BLL.Configuration;
using spr311_web_api.BLL.Dtos.Account;
using spr311_web_api.BLL.Services.EmailService;
using spr311_web_api.BLL.Services.Jwt;
using spr311_web_api.DAL.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace spr311_web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly JwtSettings _jwtSettings;
        private readonly IJwtService _jwtService;

        public AccountService(UserManager<AppUser> userManager, IEmailService emailService, IOptions<JwtSettings> jwtOptions, IJwtService jwtService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _jwtSettings = jwtOptions.Value;
            _jwtService = jwtService;
        }

        public async Task<ServiceResponse> RegisterAsync(RegisterDto dto)
        {
            if(!await IsUniqueEmailAsync(dto.Email))
            {
                return ServiceResponse.Error($"Пошта {dto.Email} зайнята");
            }
            if (!await IsUniqueUserNameAsync(dto.UserName))
            {
                return ServiceResponse.Error($"Ім'я {dto.UserName} зайняте");
            }

            var user = new AppUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if(result.Succeeded)
            {
                await SendEmailConfirmMessageAsync(user);

                // Generate jwt token
                var claims = new Claim[]
                {
                new Claim("userId", user.Id),
                new Claim("email", user.Email ?? ""),
                new Claim("userName", user.UserName ?? "")
                };

                var bytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
                var securityKey = new SymmetricSecurityKey(bytes);

                var securityToken = new JwtSecurityToken(
                    audience: _jwtSettings.Audience,
                    issuer: _jwtSettings.Issuer,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpMinutes),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                    );

                string jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

                return ServiceResponse.Success("Успішна реєстрація", jwtToken);
            }

            return ServiceResponse.Error(result.Errors.First().Description); ;
        }

        private async Task SendEmailConfirmMessageAsync(AppUser user)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            byte[] bytes = Encoding.UTF8.GetBytes(token);
            token = Convert.ToBase64String(bytes);

            string htmlPath = Path.Combine(Settings.RootPath ?? "/", "templates", "html", "confirm_email.html");
            string html = File.ReadAllText(htmlPath);
            string url = $"https://localhost:7080/api/account/confirmEmail?userId={user.Id}&token={token}";
            html = html.Replace("action_url", url);

            await _emailService.SendMessageAsync(user.Email, "Підтвердження пошти", html, true);
        }

        private async Task<bool> IsUniqueEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null;
        }

        private async Task<bool> IsUniqueUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user == null;
        }

        public async Task<ServiceResponse> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if(user == null)
            {
                return ServiceResponse.Error($"Користувача з іменем {dto.UserName} не знайдено");
            }

            var passwordResult = await _userManager.CheckPasswordAsync(user, dto.Password);

            if(!passwordResult)
            {
                return ServiceResponse.Error("Пароль вказано не вірно");
            }

            var tokens = await _jwtService.GenerateTokensAsync(user);

            return ServiceResponse.Success("Успішний вхід", tokens.Payload);
        }
    }
}

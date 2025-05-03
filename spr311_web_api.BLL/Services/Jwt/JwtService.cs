using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using spr311_web_api.BLL.Configuration;
using spr311_web_api.BLL.Dtos.Auth;
using spr311_web_api.DAL.Entities;
using spr311_web_api.DAL.Entities.Identity;
using spr311_web_api.DAL.Repositories.Auth;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace spr311_web_api.BLL.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<AppUser> _userManager;

        public JwtService(IOptions<JwtSettings> jwtOptions, IAuthRepository authRepository, UserManager<AppUser> userManager)
        {
            _jwtSettings = jwtOptions.Value;
            _authRepository = authRepository;
            _userManager = userManager;
        }

        public async Task<ServiceResponse> GenerateTokensAsync(AppUser user)
        {
            var accessToken = await GenerateAccessTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            await SaveRefreshTokenAsync(refreshToken, user.Id, accessToken.Id);

            string jwt = new JwtSecurityTokenHandler().WriteToken(accessToken);

            var payload = new JwtDto
            {
                AccessToken = jwt,
                RefreshToken = refreshToken
            };

            return ServiceResponse.Success("Tokens generated", payload);
        }

        public async Task<ServiceResponse> RefreshTokensAsync(JwtDto dto)
        {
            var storedToken = await _authRepository
                .GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Token == dto.RefreshToken);

            if (storedToken == null 
                || storedToken.IsUsed
                || storedToken.ExpiredDate < DateTime.UtcNow)
            {
                return ServiceResponse.Error("Invalid token");
            }

            var principals = GetPrincipals(dto.AccessToken);

            var accessTokenId = principals.Claims
                .Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            if(storedToken.JwtId != accessTokenId)
            {
                return ServiceResponse.Error("Invalid token");
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId);

            if(user == null)
            {
                return ServiceResponse.Error("Invalid token");
            }

            storedToken.IsUsed = true;
            await _authRepository.UpdateAsync(storedToken);

            var response = await GenerateTokensAsync(user);
            return response;
        }

        private ClaimsPrincipal GetPrincipals(string accessToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero,
                ValidAudience = _jwtSettings.Audience,
                ValidIssuer = _jwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey ?? ""))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            
            var principals = tokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if(jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                throw new SecurityTokenException("Invalid access token");
            }

            return principals;
        }

        private async Task<JwtSecurityToken> GenerateAccessTokenAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id),
                new Claim("email", user.Email ?? ""),
                new Claim("userName", user.UserName ?? "")
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            if(userRoles.Count > 0)
            {
                foreach (var role in userRoles)
                {
                    claims.Add(new Claim("roles", role));
                }
            }
            else
            {
                claims.Add(new Claim("roles", "user"));
            }

                var bytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey ?? "");
            var securityKey = new SymmetricSecurityKey(bytes);

            var securityToken = new JwtSecurityToken(
                audience: _jwtSettings.Audience,
                issuer: _jwtSettings.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpMinutes),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

            return securityToken;
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetNonZeroBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private async Task SaveRefreshTokenAsync(string token, string userId, string jwtId)
        {
            var entity = new RefreshTokenEntity
            {
                ExpiredDate = DateTime.UtcNow.AddDays(7),
                Token = token,
                UserId = userId,
                JwtId = jwtId,

            };

            await _authRepository.CreateAsync(entity);
        }
    }
}

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;

namespace CustomerOrder.API.Services.Authority
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IConfiguration _configuration;
        public AuthorityService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJwtToken(string userId)
        {
            try
            {
                // JWT ayarlarını al
                var jwtSettings = _configuration.GetSection("Jwt");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Token içeriği
                var claims = new[]
                {
                     new Claim(ClaimTypes.NameIdentifier, userId),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<int>("Expires")),
                    Issuer = jwtSettings["Issuer"],
                    Audience = jwtSettings["Audience"],
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Jwt token error: {ex.Message}");
            }
        }
    }
}

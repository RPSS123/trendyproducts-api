using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TrendyProducts.Utilities
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireMinutes;

        public JwtService(IConfiguration config)
        {
            _config = config;
            _key = _config["Jwt:Key"];
            _issuer = _config["Jwt:Issuer"];
            _audience = _config["Jwt:Audience"];
            _expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "120");
        }

        public string GenerateToken(int userId, string role)
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
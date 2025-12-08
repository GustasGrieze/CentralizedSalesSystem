using CentralizedSalesSystem.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CentralizedSalesSystem.API.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
    }
    public sealed class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryMinutes = int.TryParse(_config["JWT:ExpiryMinutes"], out var minutes)
                ? minutes
                : 60;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("businessId", user.BusinessId.ToString())
            };

            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new(ClaimTypes.Role, userRole.Role.Title));

                foreach (var rp in userRole.Role.RolePermissions)
                {
                    claims.Add(new("permission", rp.Permission.Code));
                }
            }

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

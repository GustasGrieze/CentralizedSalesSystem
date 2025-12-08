using CentralizedSalesSystem.API.Data;
using CentralizedSalesSystem.API.Models;
using CentralizedSalesSystem.API.Models.Auth;
using CentralizedSalesSystem.API.Models.Auth.enums;
using CentralizedSalesSystem.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CentralizedSalesSystem.API.Controllers
{
    [Route("auth")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CentralizedSalesDbContext _db;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(CentralizedSalesDbContext db, IConfiguration config, ITokenService tokenService, IPasswordHasher<User> passwordHasher)
        {
            _db = db;
            _config = config;
            _tokenService =tokenService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            var error_str = "Invalid email or password";
            if (user == null)
            {
                return Unauthorized(new { code = "invalid_credentials", message = error_str });
            }

            var isCorrectPassword = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if( isCorrectPassword == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { code = "invalid_credentials", message = error_str });
            }

            var activeRoles = user.UserRoles
                .Select(u => u.Role)
                .Where(r => r.Status == Status.Active)
                .ToList();

            var roleTitles = activeRoles
                .Select(r => r.Title)
                .Distinct() // Shouldn't have two of the same active roles, but using distinct just in case
                .ToList();

            var permissionCodes = activeRoles
                .SelectMany(r => r.RolePermissions)
                .Where(rp => rp.Permission.Status == Status.Active)
                .Select(rp => rp.Permission)
                .Distinct()
                .ToList();

            var token = _tokenService.GenerateAccessToken(user);

            return Ok(new TokenResponse(token));

        }


    }
}

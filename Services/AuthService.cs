using englishCardsAPI.DTOs;
using englishCardsAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace englishCardsAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResultDto> RegisterAsync([FromBody] RegisterUserDto registerUserDto)
        {
            var userExists = await _userManager.FindByEmailAsync(registerUserDto.Email);
            if (userExists != null)
            {
                return new AuthResultDto { Success = false, Message = $"User creation failed: The email is already taken." };
            }

            var user = new AppUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName,
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                return new AuthResultDto { Success = false, Message = $"User creation failed:\n{errors}" };
            }

            return new AuthResultDto { Success = true, Message = "User created successfully!" };
        }

        public async Task<AuthResultDto> LoginAsync([FromBody] LoginUserDto loginUserDto)
        {
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginUserDto.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new AuthResultDto
                {
                    Success = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                };
            }

            return new AuthResultDto { Success = false, Message = "Invalid email or password." };
        }
    }
}

using englishCardsAPI.DTOs;
using englishCardsAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using englishCardsAPI.Utils;

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

                string? refreshToken = RefreashTokenGenerator.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userManager.UpdateAsync(user);

                return new AuthResultDto
                {
                    Success = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    RefreshToken = refreshToken
                };
            }

            return new AuthResultDto { Success = false, Message = "Invalid email or password." };
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var deletedUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (deletedUser == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(deletedUser);
            return result.Succeeded;
        }

        public async Task<AuthResultDto> RefreshSessionAsync(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new AuthResultDto { Success = false, Message = "Invalid or expired refresh token." };
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(15),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var newRefreshToken = RefreashTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);
                
            return new AuthResultDto
            {
                Success = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = newRefreshToken
            };
        }
    }
}

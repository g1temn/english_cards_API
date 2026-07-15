using englishCardsAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace englishCardsAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync([FromBody] RegisterUserDto registerUserDto);
        Task<AuthResultDto> LoginAsync([FromBody] LoginUserDto loginUserDto);
        Task<bool> DeleteUserAsync(int userId);
    }
}

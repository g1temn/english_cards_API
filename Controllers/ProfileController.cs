using englishCardsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace englishCardsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("generalInfo")]
        [Authorize]
        public async Task<IActionResult> GetGeneralInfo()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Invalid token data.");
            }

            var generalInfo = await _profileService.GetGeneralInfoByUserIdAsync(userId);
            if (generalInfo == null)
            {
                return NotFound("User not found.");
            }

            return Ok(generalInfo);
        }

        [HttpGet("userProfileInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserProfileInfo()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Invalid token data.");
            }

            var userProfileData = await _profileService.GetUserProfileDataByUserIdAsync(userId);
            if (userProfileData == null)
            {
                return NotFound("User not found.");
            }
            return Ok(userProfileData);
        }
    }
}

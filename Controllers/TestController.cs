using englishCardsAPI.DTOs;
using englishCardsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace englishCardsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTests()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Invalid token data.");
            }

            var tests = await _testService.GetTestsByUserIdAsync(userId);

            return Ok(tests);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTest([FromBody] TestCreationDto createTestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Invalid token data.");
            }

            var createdTest = await _testService.CreateTestAsync(userId, createTestDto);

            return Ok(createdTest);
            
        }
    }
}

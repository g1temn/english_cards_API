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
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCards()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Invalid token data.");
            }

            var cards = await _cardService.GetCardsByUserIdAsync(userId);

            return Ok(cards);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCard([FromBody] CardCreationDto createCardDto)
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

            var createdCard = await _cardService.CreateCardAsync(userId, createCardDto);
            return Ok(createdCard);
        }
    }
}

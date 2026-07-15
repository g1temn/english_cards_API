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

        [HttpPut("{cardId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCard(int cardId, [FromBody] CardUpdatingDto updateCardDto)
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

            var updatedCard = await _cardService.UpdateCardAsync(userId, cardId, updateCardDto);
            if (updatedCard == null)
            {
                return NotFound("Card not found.");
            }

            return Ok(updatedCard);
        }

        [HttpDelete("{cardId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCard(int cardId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Invalid token data.");
            }

            var isDeleted = await _cardService.DeleteCardAsync(userId, cardId);
            if (!isDeleted)
            {
                return NotFound("Card not found.");
            }

            return Ok("Card deleted successfully.");
        }
    }
}

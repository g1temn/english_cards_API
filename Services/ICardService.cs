using englishCardsAPI.DTOs;
using englishCardsAPI.Entities;

namespace englishCardsAPI.Services
{
    public interface ICardService
    {
        Task<IEnumerable<CardResponseDto>> GetCardsByUserIdAsync(int userId);

        Task<CardResponseDto> CreateCardAsync(int userId, CardCreationDto createCardDto);

        Task<CardResponseDto> UpdateCardAsync(int userId, int cardId, CardUpdatingDto updateCardDto);

        Task<bool> DeleteCardAsync(int userId, int cardId);
    }
}

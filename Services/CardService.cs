using englishCardsAPI.Data;
using englishCardsAPI.DTOs;
using englishCardsAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace englishCardsAPI.Services
{
    public class CardService : ICardService
    {
        private readonly AppDbContext _context;

        public CardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CardResponseDto>> GetCardsByUserIdAsync(int userId)
        {
            var cards = await _context.Cards
                .Where(c => c.UserId == userId)
                .Select(c => new CardResponseDto
                {
                    CardId = c.CardId,
                    Title = c.Title,
                    Phoneitcs = c.Phoneitcs,
                    Meaning = c.Meaning
                })
                .ToListAsync();

            return cards;
        }

        public async Task<CardResponseDto> CreateCardAsync(int userId, CardCreationDto createCardDto)
        {
            var newCard = new Card
            {
                UserId = userId,
                Title = createCardDto.Title,
                Phoneitcs = createCardDto.Phoneitcs,
                Meaning = createCardDto.Meaning
            };
            
            await _context.Cards.AddAsync(newCard);
            await _context.SaveChangesAsync();

            return new CardResponseDto
            {
                CardId = newCard.CardId,
                Title = newCard.Title,
                Phoneitcs = newCard.Phoneitcs,
                Meaning = newCard.Meaning
            };
        }

        public async Task<CardResponseDto> UpdateCardAsync(int userId, int cardId, CardUpdatingDto updateCardDto)
        {
            var updatedCard = await _context.Cards.FirstOrDefaultAsync(c => c.CardId == cardId && c.UserId == userId);

            if (updatedCard == null) return null;

            updatedCard.Title = updateCardDto.Title;
            updatedCard.Phoneitcs = updateCardDto.Phonetics;
            updatedCard.Meaning = updateCardDto.Meaning;

            await _context.SaveChangesAsync();

            return new CardResponseDto
            {
                CardId = updatedCard.CardId,
                Title = updatedCard.Title,
                Phoneitcs = updatedCard.Phoneitcs,
                Meaning = updatedCard.Meaning
            };
        }

        public async Task<bool> DeleteCardAsync(int userId, int cardId)
        {
            var deletedCard = await _context.Cards.FirstOrDefaultAsync(c => c.CardId == cardId && c.UserId == userId);

            if (deletedCard == null) return false;

            _context.Cards.Remove(deletedCard);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

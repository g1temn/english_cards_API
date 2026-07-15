using englishCardsAPI.Data;
using englishCardsAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace englishCardsAPI.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;

        public ProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GeneralInfoDto> GetGeneralInfoByUserIdAsync(int userId)
        {
            int totalCards = await _context.Cards
                .Where(c => c.UserId == userId)
                .CountAsync();

            var testStats = await _context.Tests
                .Where(t => t.UserId == userId)
                .GroupBy(t => t.UserId)
                .Select(g => new
                {
                    Count = g.Count(),
                    Average = g.Average(t => (double?)t.Score) ?? 0
                })
                .FirstOrDefaultAsync();

            int testsTaken = testStats?.Count ?? 0;
            double avgScore = testStats?.Average ?? 0;

            return new GeneralInfoDto
            {
                TotalCards = totalCards,
                TestsTaken = testsTaken,
                AvgScore = avgScore
            };
        }

        public async Task<UserProfileDataDto> GetUserProfileDataByUserIdAsync(int userId)
        {
            var userProfileDataDto = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileDataDto
                {
                    UserName = u.UserName ?? string.Empty,
                    Email = u.Email ?? string.Empty,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();

            return userProfileDataDto;
        }
    }
}

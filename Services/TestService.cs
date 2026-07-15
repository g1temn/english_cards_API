using englishCardsAPI.Data;
using englishCardsAPI.DTOs;
using englishCardsAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace englishCardsAPI.Services
{
    public class TestService : ITestService
    {
        private readonly AppDbContext _context;

        public TestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TestResponseDto>> GetTestsByUserIdAsync(int userId)
        {
            var testDtos = await _context.Tests
                .Where(test => test.UserId == userId)
                .OrderByDescending(test => test.TakenAt)
                .Take(5)
                .Select(test => new TestResponseDto
                {
                    Score = test.Score,
                    TakenAt = test.TakenAt
                })
                .ToListAsync();

            return testDtos;
        }

        public async Task<TestResponseDto> CreateTestAsync(int userId, TestCreationDto createTestDto)
        {
            var newTest = new Test
            {
                UserId = userId,
                Score = createTestDto.Score,
                TakenAt = DateTime.UtcNow
            };

            await _context.Tests.AddAsync(newTest);
            await _context.SaveChangesAsync();

            return new TestResponseDto
            {
                Score = newTest.Score,
                TakenAt = newTest.TakenAt
            };
        }        
    }
}

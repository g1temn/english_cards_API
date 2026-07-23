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
                    TestId = test.TestId,
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
                TestId = newTest.TestId,
                Score = newTest.Score,
                TakenAt = newTest.TakenAt
            };
        }

        public async Task<IEnumerable<TestQuestionDto>> GetTestQuestionsAsync(int userId)
        {
            var userCards = await _context.Cards
                .Where(card => card.UserId == userId)
                .ToListAsync();

            var testQuestions = new List<TestQuestionDto>();

            if(userCards.Count >= 10)
            {
                foreach (var card in userCards)
                {
                    var answers = userCards
                        .Where(c => c.CardId != card.CardId)
                        .OrderBy(c => Random.Shared.Next())
                        .Take(3)
                        .Select(c => c.Meaning)
                        .ToList();
                    answers.Add(card.Meaning);
                    answers = answers.OrderBy(a => Random.Shared.Next()).ToList();
                    testQuestions.Add(new TestQuestionDto
                    {
                        TestQuestionId = card.CardId,
                        QuestionedWord = card.Title,
                        Answers = answers,
                        RightAnswer = card.Meaning
                    });
                }
            }

            testQuestions = testQuestions.OrderBy(q => Random.Shared.Next()).Take(10).ToList();
            return testQuestions;
        }
    }
}

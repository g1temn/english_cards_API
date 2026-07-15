using englishCardsAPI.DTOs;

namespace englishCardsAPI.Services
{
    public interface ITestService
    {
        Task<IEnumerable<TestResponseDto>> GetTestsByUserIdAsync(int userId);

        Task<TestResponseDto> CreateTestAsync(int userId, TestCreationDto createTestDto);
    }
}

using englishCardsAPI.DTOs;

namespace englishCardsAPI.Services
{
    public interface IProfileService
    {
        Task<GeneralInfoDto> GetGeneralInfoByUserIdAsync(int userId);
        Task<UserProfileDataDto> GetUserProfileDataByUserIdAsync(int userId);
    }
}

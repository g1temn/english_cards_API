namespace englishCardsAPI.DTOs
{
    public class UserProfileDataDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

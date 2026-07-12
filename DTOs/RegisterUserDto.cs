using System.ComponentModel.DataAnnotations;

namespace englishCardsAPI.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

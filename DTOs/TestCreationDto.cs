using System.ComponentModel.DataAnnotations;

namespace englishCardsAPI.DTOs
{
    public class TestCreationDto
    {
        [Required]
        public int Score { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace englishCardsAPI.DTOs
{
    public class CardCreationDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Phoneitcs { get; set; } = string.Empty;

        [Required]
        public string Meaning { get; set; } = string.Empty;
    }
}

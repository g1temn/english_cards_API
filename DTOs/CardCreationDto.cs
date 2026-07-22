using System.ComponentModel.DataAnnotations;

namespace englishCardsAPI.DTOs
{
    public class CardCreationDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Phonetics { get; set; } = string.Empty;

        [Required]
        public string Meaning { get; set; } = string.Empty;
    }
}

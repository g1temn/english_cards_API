using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace englishCardsAPI.Entities
{
    public class Card
    {
        public int CardId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Phoneitcs { get; set; } = string.Empty;
        
        [Required]
        public string Meaning { get; set; } = string.Empty;

        [Required]
        [ForeignKey("AppUser")]
        public int UserId { get; set; }
        
        public AppUser? AppUser { get; set; }
    }
}

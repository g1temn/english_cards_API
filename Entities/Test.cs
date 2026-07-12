using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace englishCardsAPI.Entities
{
    public class Test
    {
        public int TestId { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public DateTime TakenAt { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("AppUser")]
        public int UserId { get; set; }

        public AppUser? AppUser { get; set; }
    }
}

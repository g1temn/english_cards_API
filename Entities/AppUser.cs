using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace englishCardsAPI.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public ICollection<Card>? Cards { get; set; }
        public ICollection<Test>? Tests { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

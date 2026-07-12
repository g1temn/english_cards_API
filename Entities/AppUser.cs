using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace englishCardsAPI.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string UserSurname { get; set; } = string.Empty;
        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<Test> Tests { get; set; } = new List<Test>();
    }
}

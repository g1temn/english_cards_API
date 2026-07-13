namespace englishCardsAPI.DTOs
{
    public class CardResponseDto
    {
        public int CardId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Phoneitcs { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
    }
}

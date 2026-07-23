using System.ComponentModel.DataAnnotations;

namespace englishCardsAPI.DTOs
{
    public class TestQuestionDto
    {
        [Required]
        public int TestQuestionId { get; set; }

        [Required]
        public string QuestionedWord { get; set; } = string.Empty;
        
        [Required]
        public IEnumerable<string> Answers { get; set; } = new List<string>();
        
        [Required]
        public string RightAnswer { get; set; } = string.Empty;
    }
}

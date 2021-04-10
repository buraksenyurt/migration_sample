using System.ComponentModel.DataAnnotations;

namespace ClassicGames.Models
{
    public class GameReview
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        [Range(1,10)]
        public int? Rating { get; set; }
        public string Review { get; set; }
        [Required]
        public string User { get; set; }
        public int CommentScore { get; set; }
    }
}

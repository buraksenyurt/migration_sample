﻿using System.ComponentModel.DataAnnotations;

namespace ClassicGames.Models
{
    public class GameReview
    {
        public int GameReviewID { get; set; }
        public Game Game { get; set; }
        [Range(1,10)]
        public int? Rating { get; set; }
        public string Review { get; set; }
        [Required]
        public string Motto { get; set; }
    }
}
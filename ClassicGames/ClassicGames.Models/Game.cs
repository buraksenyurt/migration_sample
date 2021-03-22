using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClassicGames.Models
{
    public class Game
    {
        public int GameID { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        [JsonIgnore]
        public byte[] Photo { get; set; }
        public string Publisher { get; set; }
        public ICollection<GameReview> Reviews { get; set; }
    }
}

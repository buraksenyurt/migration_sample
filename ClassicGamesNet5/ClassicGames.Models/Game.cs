using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClassicGames.Models
{
    public class Game
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Summary { get; set; }
        public DateTime PublishDate { get; set; }
        [JsonIgnore]
        public byte[] Photo { get; set; }
        public string Developers { get; set; }
        public ICollection<GameReview> Reviews { get; set; }
    }
}

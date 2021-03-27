using ClassicGames.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;

namespace ClassicGames.DAL
{
    public class CommodoreDBContext
        : DbContext
    {
        public CommodoreDBContext(DbContextOptions<CommodoreDBContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<GameReview> GameReviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .ToTable("Game");

            modelBuilder.Entity<GameReview>()
                .ToTable("GameReview");

            modelBuilder.Entity<GameReview>()
                .HasOne(g => g.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey("Game_Id");

            Log.Warning("Veriler için tohumlama operasyonu başlatılıyor...");

            var a_few_games = new[]
            {
                new Game
                {
                    Id=1,
                    Name="The Last Ninja",
                    Summary="The Last Ninja is an action-adventure game originally developed and published by System 3 in 1987 for the Commodore 64. Other format conversions...",
                    PublishDate=new DateTime(1987,1,1),
                    Developers="System 3, Eclipse Software Design",
                    Photo=ImageLoader.GetGameCover("the_last_ninja.jpg")
                },
                new Game
                {
                    Id=2,
                    Name="Paperboy",
                    Summary="Paperboy is an arcade game developed and published by Atari Games. It was released in North America in April 1985. The player takes the role of a paperboy who delivers a fictional newspaper called 'The Daily Sun' along a suburban street on his bicycle...",
                    PublishDate=new DateTime(1984,1,1),
                    Developers=" Midway Games West Inc, Mindscape, Atari...",
                    Photo=ImageLoader.GetGameCover("paper_boy.png")
                }
            };
            modelBuilder.Entity<Game>().HasData(a_few_games);

            modelBuilder.Entity<GameReview>().HasData(new
            {
                GameId=1,
                Id=1,
                User = "Retro Baba",
                Rating = 8,
                Review = "Çocukluğumun en güzel oyunlarından birisidir."
            });
            modelBuilder.Entity<GameReview>().HasData(new
            {
                GameId = 1,
                Id = 2,
                UsUser = "Baracuda 1234",
                Rating = 7,
                Review = "Çocukluk arkadaşımla başında saatler geçirdiğimiz oyundur."
            });
            modelBuilder.Entity<GameReview>().HasData(new
            {
                GameId = 2,
                Id = 3,
                User = "Gold Gamer",
                Rating = 9,
                Review = "Ah o posta kutusuna her seferinde aldanıp çarpardım. Sırf bu oyun için eski bir Commodore bulup satın aldım ve kurdum. Düşün bi, 37 ekran TV, kaset çalar, anten girişi, kafa ayarı..."
            });
        }
    }
}

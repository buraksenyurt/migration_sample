using ClassicGames.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ClassicGames.DAL
{
    public class CommodoreDBInitializer
        : CreateDatabaseIfNotExists<CommodoreDBContext>
    {
        protected override void Seed(CommodoreDBContext context)
        {
            Log.Warning("Db Seed operasyonu başlatılıyor");

            var a_few_games = new List<Game>
            {
                new Game
                {
                   Name="The Last Ninja",
                   Summary="The Last Ninja is an action-adventure game originally developed and published by System 3 in 1987 for the Commodore 64. Other format conversions...",
                   PublishDate=new DateTime(1987,1,1),
                   Developers="System 3, Eclipse Software Design",
                   Photo=ImageLoader.GetGameCover("the_last_ninja.jpg"),
                   Reviews=new List<GameReview>
                   {
                        new GameReview
                        {
                            User="Retro Baba",
                            Rating=8,
                            Review="Çocukluğumun en güzel oyunlarından birisidir."
                        },
                        new GameReview
                        {
                            User="Baracuda 1234",
                            Rating=7,
                            Review="Çocukluk arkadaşımla başında saatler geçirdiğimiz oyundur."
                        }
                   }
                },
                new Game
                {
                   Name="Paperboy",
                   Summary="Paperboy is an arcade game developed and published by Atari Games. It was released in North America in April 1985. The player takes the role of a paperboy who delivers a fictional newspaper called 'The Daily Sun' along a suburban street on his bicycle...",
                   PublishDate=new DateTime(1984,1,1),
                   Developers=" Midway Games West Inc, Mindscape, Atari...",
                   Photo=ImageLoader.GetGameCover("paper_boy.png"),
                   Reviews=new List<GameReview>
                   {
                        new GameReview
                        {
                            User="Gold Gamer",
                            Rating=9,
                            Review="Ah o posta kutusuna her seferinde aldanıp çarpardım. Sırf bu oyun için eski bir Commodore bulup satın aldım ve kurdum. Düşün bi, 37 ekran TV, kaset çalar, anten girişi, kafa ayarı..."
                        }
                   }
                }
            };
            context.Games.AddRange(a_few_games);

            base.Seed(context);
        }
    }
}

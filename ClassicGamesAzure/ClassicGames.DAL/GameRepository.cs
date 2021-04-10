using ClassicGames.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ClassicGames.DAL
{
    public class GameRepository
        : IGameRepository
    {
        private readonly DbContextOptions<CommodoreDBContext> _dbContextOptions;
        public GameRepository(DbContextOptions<CommodoreDBContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }
        public GameReview AddReview(int id, GameReview gameReview)
        {
            // Tüm metotlarda DbContext nesnesi using ile birlikte kullanılmıştır
            // EF Core birden fazla Thread üstünden gelen operasyonların aynı context içerisinde ele alınmasına izin vermez
            // Bu nedenle işlemler kendi bağımsız alanları içinde yapılır.
            using var context = new CommodoreDBContext(_dbContextOptions);
            if (gameReview.Id > 0 || gameReview.Game != null)
                throw new DataException("Değerlendirme eklenemedi. Id 0 olmalı");

            var game = GetGameById(id, context);
            if (game == null)
                throw new DataException($"Üzgünüz. {id} numaralı oyunu bulamadık.");

            game.Reviews.Add(gameReview);
            context.SaveChanges();
            return gameReview;
        }

        public Game GetGameById(int? id, CommodoreDBContext context)
        {
            if (!id.HasValue)
                return null;
            return context.Games.Include(g => g.Reviews).FirstOrDefault(g => g.Id == id.Value);
        }

        public void Delete(int Id)
        {
            using var context = new CommodoreDBContext(_dbContextOptions);
            var game = context.Games.FirstOrDefault(g => g.Id == Id);
            if (game == null)
                throw new DataException($"Üzgünüm dostum. {Id} ile bir oyun bulamadık.");
            context.Games.Remove(game);
            context.SaveChanges();
        }

        public void DeleteReview(int id)
        {
            using var context = new CommodoreDBContext(_dbContextOptions);
            GameReview review = GetReviewById(id);
            if (review == null)
                throw new DataException($"{id} nolu bir oyun yorumu bulamadım.");

            context.GameReviews.Remove(review);
            context.SaveChanges();
        }

        public IEnumerable<Game> GetAll()
        {
            using var context = new CommodoreDBContext(_dbContextOptions);
            return context
                .Games
                .Include(g => g.Reviews)
                .ToList();
        }

        public Game GetById(int? Id)
        {
            using var context = new CommodoreDBContext(_dbContextOptions);
            if (!Id.HasValue)
                return null;

            return context
                .Games
                .Include(g => g.Reviews)
                .FirstOrDefault(g => g.Id == Id.Value);
        }

        public GameReview GetReviewById(int? id)
        {
            using var context = new CommodoreDBContext(_dbContextOptions);
            if (!id.HasValue)
                return null;

            return context
                .GameReviews
                .Include(g => g.Game)
                .FirstOrDefault(g => g.Id == id.Value);
        }

        public GameReview GetReviewById(int? id, CommodoreDBContext context)
        {
            if (!id.HasValue)
                return null;

            return context
                .GameReviews
                .Include(g => g.Game)
                .FirstOrDefault(g => g.Id == id.Value);
        }

        public GameReview UpdateReview(GameReview gameReview)
        {
            using var context = new CommodoreDBContext(_dbContextOptions);
            var gameReviewDb = GetReviewById(gameReview.Id, context);
            gameReviewDb.User = gameReview.User;
            gameReviewDb.Review = gameReview.Review;
            gameReviewDb.Rating = gameReview.Rating;
            context.SaveChanges();
            return gameReviewDb;
        }

        public Game UpsertGame(Game game)
        {
            using var context = new CommodoreDBContext(_dbContextOptions);
            if (game.Id <= 0)
                context.Games.Add(game);
            else
                context.Entry(game).State = EntityState.Modified;

            context.SaveChanges();
            return game;
        }
    }
}

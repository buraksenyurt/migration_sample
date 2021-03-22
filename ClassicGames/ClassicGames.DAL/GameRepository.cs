using ClassicGames.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace ClassicGames.DAL
{
    public class GameRepository
        : IGameRepository
    {
        private readonly CommodoreDBContext _dbContext;
        public GameRepository(CommodoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public GameReview AddReview(int id, GameReview gameReview)
        {
            if (gameReview.Id > 0 || gameReview.Game != null)
                throw new DataException("Değerlendirme eklenemedi. Id 0 olmalı");

            var game = GetById(id);
            if (game == null)
                throw new DataException($"Üzgünüz. {id} numaralı oyunu bulamadık.");

            game.Reviews.Add(gameReview);
            _dbContext.SaveChangesAsync();
            return gameReview;
        }

        public void Delete(int Id)
        {
            var game = _dbContext.Games.FirstOrDefault(g => g.Id == Id);
            if (game == null)
                throw new DataException($"Üzgünüm dostum. {Id} ile bir oyun bulamadık.");
            _dbContext.Games.Remove(game);
            _dbContext.SaveChanges();
        }

        public void DeleteReview(int id)
        {
            GameReview review = GetReviewById(id);
            if (review == null)
                throw new DataException($"{id} nolu bir oyun yorumu bulamadım.");

            _dbContext.GameReviews.Remove(review);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Game> GetAll()
        {
            return _dbContext
                .Games
                .Include(g => g.Reviews)
                .ToList();
        }

        public Game GetById(int? Id)
        {
            if (!Id.HasValue)
                return null;

            return _dbContext
                .Games
                .Include(g => g.Reviews)
                .FirstOrDefault(g => g.Id == Id.Value);
        }

        public GameReview GetReviewById(int? id)
        {
            if (!id.HasValue)
                return null;

            return _dbContext
                .GameReviews
                .Include(g => g.Game)
                .FirstOrDefault(g => g.Id == id.Value);
        }

        public GameReview UpdateReview(GameReview gameReview)
        {
            var gameReviewDb = GetReviewById(gameReview.Id);
            gameReviewDb.User = gameReview.User;
            gameReviewDb.Review = gameReview.Review;
            gameReviewDb.Rating = gameReview.Rating;
            _dbContext.SaveChanges();
            return gameReviewDb;
        }

        public Game UpsertGame(Game game)
        {
            if (game.Id <= 0)
                _dbContext.Games.Add(game);

            _dbContext.SaveChanges();
            return game;
        }
    }
}

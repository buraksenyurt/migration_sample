using ClassicGames.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassicGames.DAL
{
    public interface IGameRepository
    {
        Game UpsertGame(Game game);
        IEnumerable<Game> GetAll();
        void Delete(int Id);
        Game GetById(int? Id);
        GameReview AddReview(int id, GameReview gameReview);
        GameReview GetReviewById(int? id);
        GameReview UpdateReview(GameReview gameReview);
        void DeleteReview(int id);
    }
}

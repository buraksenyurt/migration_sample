using ClassicGames.DAL;
using ClassicGames.Models;
//using System.Web.Mvc; //Eski
using Microsoft.AspNetCore.Mvc; //Yeni

namespace ClassicGames.WebClient.Controllers
{
    public class GameReviewsController : Controller
    {
        private IGameRepository _gameRepository;

        public GameReviewsController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        // GET: GameReviews
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest); //Eski
                return new BadRequestResult(); //Yeni
            }
            var game = _gameRepository.GetById(id);
            if (game == null)
            {
                // return HttpNotFound(); //Eski
                return NotFound();// Yeni
            }
            return View(game);
        }

        // GET: GameReviews/Create
        public ActionResult Create(int id)
        {
            var game = _gameRepository.GetById(id);
            return View(
                new GameReview
                {
                    Game = game
                });
        }

        // POST: GameReviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int gameId, [Bind("Rating,Review,User")] GameReview gameReview) // Include kaldırıldı
        {
            if (ModelState.IsValid)
            {
                _gameRepository.AddReview(gameId, gameReview);
                return RedirectToAction("Index"
                    , new
                    {
                        id = gameId
                    });
            }

            return View(gameReview);
        }

        // GET: GameReviews/Edit/1234
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest); //Eski
                return new BadRequestResult(); //Yeni
            }
            GameReview gameReview = _gameRepository.GetReviewById(id);
            if (gameReview == null)
            {
                // return HttpNotFound(); // Eski
                return NotFound(); // Yeni
            }
            return View(gameReview);
        }

        // POST: GameReviews/Edit/1234
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int gameId, [Bind("Id,Rating,Review,User")] GameReview gameReview) //Include kaldırıldı
        {
            if (ModelState.IsValid)
            {
                _gameRepository.UpdateReview(gameReview);
                return RedirectToAction("Index"
                    , new
                    {
                        id = gameId
                    });
            }
            return View(gameReview);
        }

        // GET: GameReviews/Delete/1234
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                 // return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // Eski
                return new BadRequestResult(); // Yeni
            }
            GameReview gameReview = _gameRepository.GetReviewById(id);
            if (gameReview == null)
            {
                // return HttpNotFound(); //Eski
                return NotFound(); // Yeni
            }
            return View(gameReview);
        }

        // POST: GameReviews/Delete/1234
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int gameId)
        {
            _gameRepository.DeleteReview(id);
            return RedirectToAction("Index",
                new
                {
                    id = gameId
                });
        }
    }
}
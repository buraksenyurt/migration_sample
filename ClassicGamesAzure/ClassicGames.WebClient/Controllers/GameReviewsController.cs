using ClassicGames.DAL;
using ClassicGames.Models;
//using System.Web.Mvc; //Eski
using Microsoft.AspNetCore.Mvc; //Yeni
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClassicGames.WebClient.Controllers
{
    public class GameReviewsController : Controller
    {
        private IGameRepository _gameRepository;
        private AlienistServiceSettings _alienistServiceSettings;

        public GameReviewsController(IGameRepository gameRepository, AlienistServiceSettings alienistServiceSettings)
        {
            _gameRepository = gameRepository;
            _alienistServiceSettings = alienistServiceSettings;
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
        public async Task<ActionResult> Create(int gameId, [Bind("Rating,Review,User")] GameReview gameReview) // Include kaldırıldı
        {
            if (ModelState.IsValid)
            {
                //TODO: Eklenen yorumum pozition/negatif skorunu bu örnek için nasıl değerlendirebiliriz?
                var commentScore = await AnalyzeComment(gameReview.Review);
                gameReview.CommentScore = commentScore;
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
        public async Task<ActionResult> Edit(int gameId, [Bind("Id,Rating,Review,User")] GameReview gameReview) //Include kaldırıldı
        {
            if (ModelState.IsValid)
            {
                var commentScore = await AnalyzeComment(gameReview.Review);
                gameReview.CommentScore = commentScore;

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

        /*
         Azure fonksiyonuna çağrı yapan action metodu.
         */
        public async Task<int> AnalyzeComment(string content)
        {
            // HttpClient nesnesi hazırlayıp, appSettings.json'dan aldığımız değerleri kullanarak, talep gönderiyoruz.
            var client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_alienistServiceSettings.Url}?Code={_alienistServiceSettings.AuthKey}&content={content}")
            };
            var response = await client.SendAsync(request);
            var score = await response.Content.ReadAsStringAsync();
            return Convert.ToInt32(score);
        }
    }
}
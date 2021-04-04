﻿using ClassicGames.DAL;
using ClassicGames.Models;
using System.Net;
//using System.Web.Mvc; // Eski
using Microsoft.AspNetCore.Mvc; // Yeni

namespace ClassicGames.WebClient.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameRepository _gamesRepository;

        public GamesController(IGameRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }
        // Tüm oyunları getirir 
        // GET: Games
        public ActionResult Index()
        {
            return View(_gamesRepository.GetAll());
        }

        // Get: Games/Create
        public ActionResult Create()
        {
            return View();
        }

        // Post: Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Name,Summary,PublishDate,Photo,Developers")] Game game) // Include kaldırıldı
        {
            if (ModelState.IsValid)
            {
                _gamesRepository.UpsertGame(game);
                return RedirectToAction("Index");
            }

            return View(game);
        }

        // GET: Games/Details/1234
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // Eski
                return new BadRequestResult(); //Yeni
            }
            Game game = _gamesRepository.GetById(id);
            if (game == null)
            {
                // return HttpNotFound(); // Eski
                return NotFound(); // Yeni
            }
            return View(game);
        }

        // GET: Games/Edit/1234
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // Eski
                return new BadRequestResult(); // Yeni
            }
            Game game = _gamesRepository.GetById(id);
            if (game == null)
            {
                // return HttpNotFound(); // Eski
                return NotFound(); // Yeni
            }
            return View(game);
        }

        // POST: Games/Edit/1234
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Name,Summary,PublishDate,Photo,Developers")] Game game) // Include kaldırıldı
        {
            if (ModelState.IsValid)
            {
                _gamesRepository.UpsertGame(game);

                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: Games/Delete/1234
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // Eski
                return new BadRequestResult(); // Yeni
            }
            Game game = _gamesRepository.GetById(id);
            if (game == null)
            {
                // return HttpNotFound(); // Eski
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/1234
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _gamesRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
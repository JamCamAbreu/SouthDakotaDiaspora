using Data.Models;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class GamesController : Controller
    {
        IGameData db;
        public GamesController(IGameData database)
        {
            this.db = database;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = db.GetAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Create", rc = "Games" });
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Game game)
        {
            if (ModelState.IsValid)
            {
                db.Add(game);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra="Edit", rc="Games", rid=id });
            }

            var model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Game game)
        {
            var existing = db.Get(game.ActivityId);
            if (existing == null)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                db.Update(game);
                return RedirectToAction("Index");
            }
            return View(game);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Index", rc = "Games" });
            }

            Game model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection form)
        {
            Game model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }

            db.Delete(id);
            Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Game '{model.Name}' was successfully removed.");
            return RedirectToAction("Index");
        }
    }
}
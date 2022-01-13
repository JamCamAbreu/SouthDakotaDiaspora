using Data.Models;
using Data.Services;
using Site.Models.Shows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class ShowsController : Controller
    {
        IShowData db;
        public ShowsController(IShowData database)
        {
            this.db = database;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Show> rawshows = db.GetAll();
            List<ShowInfo> shows = new List<ShowInfo>();
            foreach (Show rawshow in rawshows)
            {
                shows.Add(new ShowInfo(rawshow));
            }
            return View(shows);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Show existing = db.Get(id);
            if (existing == null)
            {
                return View("NotFound");
            }
            return View(existing);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Create", rc = "Shows" });
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Show show)
        {
            if (ModelState.IsValid)
            {
                db.Add(show);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Edit", rc = "Shows", rid = id });
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
        public ActionResult Edit(Show show)
        {
            var existing = db.Get(show.ActivityId);
            if (existing == null)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                db.Update(show);
                Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully updated Show: {show.Name}");
                return RedirectToAction("Index");
            }
            return View(show);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Index", rc = "Shows" });
            }

            Show model = db.Get(id);
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
            Show model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }

            db.Delete(id);
            Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Show '{model.Name}' was successfully removed.");
            return RedirectToAction("Index");
        }
    }
}
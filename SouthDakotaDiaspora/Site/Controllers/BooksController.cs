using Data.Models;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class BooksController : Controller
    {
        IBookData db;
        public BooksController(IBookData database)
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
                return RedirectToAction("Login", "Home", new { ra = "Create", rc = "Books" });
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                db.Add(book);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Edit", rc = "Books", rid = id });
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
        public ActionResult Edit(Book book)
        {
            var existing = db.Get(book.ActivityId);
            if (existing == null)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                db.Update(book);
                Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully updated Book: {book.Name}");
                return RedirectToAction("Index");
            }
            return View(book);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Index", rc = "Books" });
            }

            Book model = db.Get(id);
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
            Book model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }

            db.Delete(id);
            Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Book '{model.Name}' was successfully removed.");
            return RedirectToAction("Index");
        }
    }
}
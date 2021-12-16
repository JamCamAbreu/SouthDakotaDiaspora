using Data.Models;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class UsersController : Controller
    {
        IUserData db;
        public UsersController(IUserData database)
        {
            this.db = database;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var users = this.db.GetAll();
            return View(users);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                User existing = this.db.GetAll().Where(u => u.Username == user.Username).FirstOrDefault();
                if (existing != null)
                {
                    ModelState.AddModelError("Username", "The chosen username is already taken. Please choose another.");
                    return View(user);
                }

                db.Add(user);
                Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully created user: {user.LastName}, {user.FirstName} ({user.Username})");
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            User existing = db.Get(id);
            if (existing == null)
            {
                return RedirectToAction("NotFound");
            }
            return View(existing);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            User existing = db.Get(id);
            if (existing != null)
            {
                db.Delete(id);
                Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully deleted user: {existing.LastName}, {existing.FirstName} ({existing.Username})");

                if (Session["UserID"] != null && Session["UserID"].ToString() == id.ToString())
                {
                    Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"You have deleted your own account. Please create a new account or log in.");
                    Helpers.GlobalMethods.ClearSession(this.Session);
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("NotFound");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            User existing = db.Get(id);
            if (existing == null)
            {
                return RedirectToAction("NotFound");
            }
            return View(existing);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            User existing = db.Get(id);
            if (existing == null)
            {
                return RedirectToAction("NotFound");
            }
            return View(existing);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            User existing = db.Get(user.Id);
            if (existing == null)
            {
                return RedirectToAction("NotFound");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (Session["UserName"] != null && Session["UserName"].ToString() == existing.Username)
                    {
                        Helpers.GlobalMethods.UpdateSession(this.Session, user);
                    }
                    db.Update(user);

                    Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully updated user: {user.LastName}, {user.FirstName} ({user.Username})");
                    return RedirectToAction("Index");
                }
                return View(user);

            }
        }
    }
}
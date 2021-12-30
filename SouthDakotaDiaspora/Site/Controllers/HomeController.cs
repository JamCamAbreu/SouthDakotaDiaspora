using Data.Models;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        IUserData db;

        public HomeController(IUserData database)
        {
            db = database;
        }

        // Request to the root address
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                int userid;
                if (int.TryParse(Session["UserID"].ToString(), out userid))
                {
                    User user = db.Get(userid);
                    if (user != null)
                    {
                        return View(user);
                    }
                }
            }
            
            return View();
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
                Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Your account was successfully created: {user.LastName}, {user.FirstName} ({user.Username})");
                Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Please log in:");
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user, string ra="", string rc="", int? rid = null)
        {
            if (ModelState.IsValid)
            {
                User existing = db.Get(user.Username, user.Password);
                if (existing != null)
                {
                    Helpers.GlobalMethods.UpdateSession(this.Session, existing);
                    db.UpdateLoginDate(existing, DateTime.Now);

                    if (!string.IsNullOrEmpty(ra) && !string.IsNullOrEmpty(rc))
                    {
                        if (rid != null) { return RedirectToAction(ra, rc, new { id = (int)rid }); }
                        else { return RedirectToAction(ra, rc); }
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Password", "The credentials you've provided do not match a user on our system. Please try again.");
                }
            }
            return View(user);
        }


        [HttpGet]
        public ActionResult Logout()
        {
            Site.Helpers.GlobalMethods.ClearSession(this.Session);
            return RedirectToAction("LogoutSuccess");
        }

        [HttpGet]
        public ActionResult LogoutSuccess()
        {
            return View();
        }
    }
}
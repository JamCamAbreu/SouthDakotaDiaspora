﻿using Data.Models;
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
                db.Add(user);
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
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                User existing = db.Get(user.Username, user.Password);
                if (existing != null)
                {
                    Session["UserID"] = existing.Id.ToString();
                    Session["UserName"] = existing.Username;
                    Session["Role"] = existing.UserRole.ToString();
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
            if (Session["UserID"] != null)
            {
                Session.Remove("UserID");
            }
            if (Session["UserName"] != null)
            {
                Session.Remove("UserName");
            }
            if (Session["Role"] != null)
            {
                Session.Remove("Role");
            }
            return RedirectToAction("LogoutSuccess");
        }

        [HttpGet]
        public ActionResult LogoutSuccess()
        {
            return View();
        }
    }
}
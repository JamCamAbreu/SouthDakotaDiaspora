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
            //var model = db.GetAll();
            //return View(model);

            return RedirectToAction("Index", "Timeline");
        }
    }
}
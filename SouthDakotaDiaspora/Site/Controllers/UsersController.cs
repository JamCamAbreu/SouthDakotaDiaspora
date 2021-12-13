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

        // GET: Users
        public ActionResult Index()
        {
            var users = this.db.GetAll();
            return View(users);
        }
    }
}
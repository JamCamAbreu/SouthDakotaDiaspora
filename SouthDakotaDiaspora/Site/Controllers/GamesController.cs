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

        // GET: Game
        public ActionResult Index()
        {
            var model = db.GetAll();
            return View(model);
        }
    }
}
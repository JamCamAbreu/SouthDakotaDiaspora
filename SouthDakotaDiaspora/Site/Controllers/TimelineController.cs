using Data.Models;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class TimelineController : Controller
    {
        ITimelineEventData db;
        public TimelineController(ITimelineEventData database)
        {
            db = database;
        }

        // GET: Timeline
        public ActionResult Index()
        {
            var model = db.GetAll();
            return View(model);
        }

        public ActionResult Event()
        {
            var model = db.GetAll();
            return View(model);
        }

    }
}
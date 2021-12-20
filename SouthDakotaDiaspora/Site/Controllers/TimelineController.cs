using Data.Models;
using Data.Services;
using Site.Models.Timeline;
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

        [HttpGet]
        public ActionResult Index()
        {
            TimelineIndexViewModel model = new TimelineIndexViewModel();
            model.PastEvents = this.db.GetBeforeToday().ToList();
            model.TodayEvents = this.db.GetToday().ToList();
            model.FutureEvents = this.db.GetAfterToday().ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimelineEvent tevent)
        {
            if (ModelState.IsValid)
            {
                db.Add(tevent);
                Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully created event.");
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
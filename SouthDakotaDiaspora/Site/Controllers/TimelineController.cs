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
            int MAX_PER_TABLE = 8;

            List<TimelineEvent> pastEvents = this.db.GetBeforeToday().ToList();
            int i;
            for (i = 0; i < Math.Min(pastEvents.Count, MAX_PER_TABLE); i++)
            {
                TimelineEvent pastEvent = pastEvents[i];
                model.PastEvents.Add(new TimelineRow(pastEvent));
            }
            if (pastEvents.Count > MAX_PER_TABLE) { model.MorePastEvents = true; }

            List<TimelineEvent> todayEvents = this.db.GetToday().ToList();
            foreach (TimelineEvent todayEvent in todayEvents)
            {
                model.TodayEvents.Add(new TimelineRow(todayEvent));
            }

            List<TimelineEvent> futureEvents = this.db.GetAfterToday().ToList();
            for (i = 0; i < Math.Min(futureEvents.Count, MAX_PER_TABLE); i++)
            {
                TimelineEvent futureEvent = futureEvents[i];
                model.FutureEvents.Add(new TimelineRow(futureEvent));
            }
            if (futureEvents.Count > MAX_PER_TABLE) { model.MoreFutureEvents = true; }

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection form)
        {
            var model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            else
            {
                db.Delete(id);
                return RedirectToAction("Index");
            }

        }
    }
}
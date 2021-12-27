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

            TimeZoneInfo localtimezone = TimeZoneInfo.Local; // server time (mountain)
            if (Session["TimeZoneInfo"] != null && Session["TimeZoneInfo"] is TimeZoneInfo)
            {
                localtimezone = Session["TimeZoneInfo"] as TimeZoneInfo;
            }
            model.TimeZoneName = localtimezone.StandardName;

            List<TimelineEvent> pastEvents = this.db.GetBeforeToday().ToList();
            int i;
            for (i = 0; i < Math.Min(pastEvents.Count, MAX_PER_TABLE); i++)
            {
                TimelineEvent pastEvent = pastEvents[i];
                model.PastEvents.Add(new TimelineRow(pastEvent, localtimezone));
            }
            if (pastEvents.Count > MAX_PER_TABLE) { model.MorePastEvents = true; }

            List<TimelineEvent> todayEvents = this.db.GetToday().ToList();
            foreach (TimelineEvent todayEvent in todayEvents)
            {
                model.TodayEvents.Add(new TimelineRow(todayEvent, localtimezone));
            }

            List<TimelineEvent> futureEvents = this.db.GetAfterToday().ToList();
            for (i = 0; i < Math.Min(futureEvents.Count, MAX_PER_TABLE); i++)
            {
                TimelineEvent futureEvent = futureEvents[i];
                model.FutureEvents.Add(new TimelineRow(futureEvent, localtimezone));
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
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Index", rc = "Timeline" });
            }

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

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Edit", rc = "Timeline", rid = id });
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
        public ActionResult Edit(TimelineEvent tevent)
        {
            var existing = db.Get(tevent.Id);
            if (existing == null)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                db.Update(tevent);
                Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"'{tevent.Title}' updated successfully.");
                return RedirectToAction("Index");
            }
            return View(tevent);
        }

    }
}
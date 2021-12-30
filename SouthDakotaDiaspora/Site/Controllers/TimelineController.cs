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
        IGameData games;
        IShowData shows;
        IBookData books;
        IActivityData activities;
        ITimelineEventData timelineevents;
        public TimelineController(ITimelineEventData timelineevents, IGameData games, IShowData shows, IBookData books, IActivityData activities)
        {
            this.timelineevents = timelineevents;
            this.games = games;
            this.shows = shows;
            this.books = books;
            this.activities = activities;
        }

        [HttpGet]
        public ActionResult Index()
        {
            TimelineIndexViewModel model = new TimelineIndexViewModel();
            int MAX_PER_TABLE = 4;

            TimeZoneInfo localtimezone = TimeZoneInfo.Local; // server time (mountain)
            if (Session["TimeZoneInfo"] != null && Session["TimeZoneInfo"] is TimeZoneInfo)
            {
                localtimezone = Session["TimeZoneInfo"] as TimeZoneInfo;
            }
            model.TimeZoneName = localtimezone.StandardName;

            List<TimelineEvent> pastEvents = this.timelineevents.GetBeforeToday().ToList();
            int i;
            for (i = Math.Max(0, pastEvents.Count - (1 + MAX_PER_TABLE)); i < Math.Min(pastEvents.Count, MAX_PER_TABLE); i++)
            {
                TimelineEvent pastEvent = pastEvents[i];
                Activity activity = this.activities.Get(pastEvent.ActivityId);
                model.PastEvents.Add(new TimelineEventInfo(pastEvent, localtimezone, activity));
            }
            if (pastEvents.Count > MAX_PER_TABLE) { model.MorePastEvents = true; }

            List<TimelineEvent> todayEvents = this.timelineevents.GetToday().ToList();
            foreach (TimelineEvent todayEvent in todayEvents)
            {
                Activity activity = this.activities.Get(todayEvent.ActivityId);
                model.TodayEvents.Add(new TimelineEventInfo(todayEvent, localtimezone, activity));
            }

            List<TimelineEvent> futureEvents = this.timelineevents.GetAfterToday().ToList();
            for (i = 0; i < Math.Min(futureEvents.Count, MAX_PER_TABLE); i++)
            {
                TimelineEvent futureEvent = futureEvents[i];
                Activity activity = this.activities.Get(futureEvent.ActivityId);
                model.FutureEvents.Add(new TimelineEventInfo(futureEvent, localtimezone, activity));
            }
            if (futureEvents.Count > MAX_PER_TABLE) { model.MoreFutureEvents = true; }

            return View(model);
        }

        private void PopulateActivities(TimelineCreateViewModel model)
        {
            List<Activity> games = this.games.GetAll().Cast<Activity>().ToList();
            List<Activity> shows = this.shows.GetAll().Cast<Activity>().ToList();
            List<Activity> books = this.books.GetAll().Cast<Activity>().ToList();
            List<Activity> projects = new List<Activity>();
            model.GameSelection = TimelineCreateViewModel.ListToDropdown(games);
            model.ShowSelection = TimelineCreateViewModel.ListToDropdown(shows);
            model.BookSelection = TimelineCreateViewModel.ListToDropdown(books);
            model.ProjectSelection = TimelineCreateViewModel.ListToDropdown(projects);
        }


        [HttpGet]
        public ActionResult Create()
        {
            
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Create", rc = "Timeline" });
            }
            TimelineCreateViewModel model = new TimelineCreateViewModel(new TimelineEvent());
            this.PopulateActivities(model);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimelineEvent tevent, FormCollection form)
        {
            //EventType
            if (string.IsNullOrEmpty(form["ActivityType"]?.ToString() ?? ""))
            {
                ModelState.AddModelError("", "Please choose an activity type");
            }

            string ActivityID = form["ActivityID"].ToString();
            int activityid;
            if (int.TryParse(ActivityID, out activityid))
            {
                tevent.ActivityId = activityid;
            }

            Activity existing = this.activities.Get(activityid);
            if (existing == null)
            {
                ModelState.AddModelError("", "Could not locate the activity in the database");
            }

            if (!ModelState.IsValid)
            {
                TimelineCreateViewModel model = new TimelineCreateViewModel(new TimelineEvent());
                this.PopulateActivities(model);

                return View(model);
            }

            timelineevents.Add(tevent);
            Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully created event.");
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection form)
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Index", rc = "Timeline" });
            }

            var model = timelineevents.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            else
            {
                timelineevents.Delete(id);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = timelineevents.Get(id);
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

            var model = timelineevents.Get(id);
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
            var existing = timelineevents.Get(tevent.TimelineEventId);
            if (existing == null)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                timelineevents.Update(tevent);
                Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"'{tevent.Title}' updated successfully.");
                return RedirectToAction("Index");
            }
            return View(tevent);
        }

    }
}
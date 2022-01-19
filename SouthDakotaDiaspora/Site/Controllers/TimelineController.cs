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
        IProjectData projects;
        IActivityData activities;
        IUserData users;
        ITimelineEventData timelineevents;
        DiscordNotifier discordNotifier;
        public TimelineController(ITimelineEventData timelineevents, IGameData games, IShowData shows, IBookData books, IProjectData projects, IUserData users, IActivityData activities, DiscordNotifier notifier)
        {
            this.timelineevents = timelineevents;
            this.games = games;
            this.shows = shows;
            this.books = books;
            this.projects = projects;
            this.users = users;
            this.activities = activities;
            this.discordNotifier = notifier;
        }

        [HttpGet]
        public ActionResult Index()
        {
            TimelineIndexViewModel model = new TimelineIndexViewModel();
            int MAX_PER_TABLE = 6;

            TimeZoneInfo localtimezone = TimeZoneInfo.Local; // server time (mountain)
            if (Session["TimeZoneInfo"] != null && Session["TimeZoneInfo"] is TimeZoneInfo)
            {
                localtimezone = Session["TimeZoneInfo"] as TimeZoneInfo;
            }
            model.TimeZoneName = localtimezone.StandardName;

            List<TimelineEvent> allPastEvents = this.timelineevents.GetBeforeToday().ToList();
            List<TimelineEvent> pastEvents = allPastEvents.Take(MAX_PER_TABLE).Reverse().ToList();
            int i;

            foreach (TimelineEvent pastEvent in pastEvents)
            {
                Activity activity = this.activities.Get(pastEvent.ActivityId);
                model.PastEvents.Add(new TimelineEventInfo(pastEvent, localtimezone, activity));
            }
            if (allPastEvents.Count > MAX_PER_TABLE) { model.MorePastEvents = true; }

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
            List<Activity> projects = this.projects.GetAll().Cast<Activity>().ToList();
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
            else
            {
                tevent.Activity = existing;
            }

            if (!ModelState.IsValid)
            {
                TimelineCreateViewModel model = new TimelineCreateViewModel(new TimelineEvent());
                this.PopulateActivities(model);
                return View(model);
            }

            int userid;
            if (this.Session["UserID"] != null && int.TryParse(this.Session["UserID"].ToString(), out userid))
            {
                User curLoggedIn = this.users.Get(userid);
                if (curLoggedIn != null)
                {
                    tevent.Host = curLoggedIn;
                    tevent.Users.Add(curLoggedIn);
                }
            }

            tevent.MaxAttendees = 0;
            if (string.IsNullOrEmpty(form["PlayerLimitUnlimited"]) && !string.IsNullOrEmpty(form["PlayerLimit"]))
            {
                int max;
                if (int.TryParse(form["PlayerLimit"], out max))
                {
                    tevent.MaxAttendees = max;
                }
            }

            if (!string.IsNullOrEmpty(form["NotifyCreated"]))
            {
                _ = this.discordNotifier.SendCreatedEvent(tevent, tevent.MaxAttendees);
            }

            if (string.IsNullOrEmpty(form["NotifyOneHour"]))
            {
                tevent.SentNotificationSoon = true; // this will prevent sending a message
            }
            if (string.IsNullOrEmpty(form["NotifyStarting"]))
            {
                tevent.SentNotificationStarting = true; // this will prevent sending a message
            }

            timelineevents.Add(tevent);
            Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully created event.");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult JoinEvent(int id)
        {
            TimelineEvent existing = timelineevents.Get(id);
            int userid;
            if (existing != null && this.Request.QueryString["userid"] != null && int.TryParse(this.Request.QueryString["userid"].ToString(), out userid))
            {

                User existinguser = users.Get(userid);
                if (existinguser != null)
                {
                    this.timelineevents.AddUserToEvent(existing, existinguser);
                    Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully joined event");
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CancelEvent(int id)
        {
            TimelineEvent existing = timelineevents.Get(id);
            int userid;
            if (existing != null && this.Request.QueryString["userid"] != null && int.TryParse(this.Request.QueryString["userid"].ToString(), out userid))
            {

                User existinguser = users.Get(userid);
                if (existinguser != null)
                {
                    this.timelineevents.RemoveUserFromEvent(existing, existinguser);
                    Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully cancelled event");
                    return RedirectToAction("Index");
                }
            }
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
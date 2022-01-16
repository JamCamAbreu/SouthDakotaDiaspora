using Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class SqlTimelineEventData : ITimelineEventData
    {
        private readonly SouthDakotaDiasporaDbContext database;
        public SqlTimelineEventData(SouthDakotaDiasporaDbContext db)
        {
            this.database = db;
        }

        public void Add(TimelineEvent timelineEvent)
        {
            this.database.TimelineEvents.Add(timelineEvent);
            this.database.SaveChanges();
        }

        public void Delete(int id)
        {
            TimelineEvent timelineEvent = database.TimelineEvents.Find(id);
            if (timelineEvent != null)
            {
                database.TimelineEvents.Remove(timelineEvent);
                database.SaveChanges();
            }
        }

        public TimelineEvent Get(int id)
        {
            return database.TimelineEvents
                .Where(t => t.TimelineEventId == id)
                .Include(t => t.Users)
                .Include(t => t.Activity)
                .FirstOrDefault();
        }

        public IEnumerable<TimelineEvent> GetAll()
        {
            return database.TimelineEvents
                .Include(t => t.Users)
                .Include(t => t.Activity)
                .ToList();
        }

        public IEnumerable<TimelineEvent> GetPendingNotifySoonEvents()
        {
            List<TimelineEvent> items = database.TimelineEvents
                .Include(t => t.Users)
                .Include(t => t.Activity)
                .ToList();
            return items.Where(tevent => tevent.StartTime <= DateTime.Now.AddHours(1) && tevent.EndTime > DateTime.Now && tevent.SentNotificationSoon == false);
        }
        public IEnumerable<TimelineEvent> GetPendingNotifyStartingEvents()
        {
            List<TimelineEvent> items = database.TimelineEvents
                .Include(t => t.Users)
                .Include(t => t.Activity)
                .ToList();
            return items.Where(tevent => tevent.StartTime <= DateTime.Now && tevent.EndTime > DateTime.Now && tevent.SentNotificationStarting == false);
        }

        public IEnumerable<TimelineEvent> GetBeforeToday()
        {
            List<TimelineEvent> items = database.TimelineEvents
                .Include(t => t.Users)
                .Include(t => t.Activity)
                .ToList();
            return items.Where(tevent => tevent.EndTime < DateTime.Now).OrderBy(tevent => tevent.StartTime);
        }
        public IEnumerable<TimelineEvent> GetToday()
        {
            DateTime endofday = DateTime.Now.Date.AddDays(1);
            List<TimelineEvent> items = database.TimelineEvents
                .Include(t => t.Users)
                .Include(t => t.Activity)
                .ToList();
            return items.Where(tevent => tevent.EndTime >= DateTime.Now && tevent.EndTime < endofday).OrderBy(tevent => tevent.StartTime);
        }
        public IEnumerable<TimelineEvent> GetAfterToday()
        {
            DateTime endofday = DateTime.Now.Date.AddDays(1);
            List<TimelineEvent> items = database.TimelineEvents
                .Include(t => t.Users)
                .Include(t => t.Activity)
                .ToList();
            return items.Where(tevent => tevent.StartTime > endofday).OrderBy(tevent => tevent.StartTime);
        }
        public void Update(TimelineEvent timelineEvent)
        {
            if (timelineEvent == null) return;
            TimelineEvent existing = this.Get(timelineEvent.TimelineEventId);
            if (existing != null)
            {
                existing.Title = timelineEvent.Title;
                existing.StartTime = timelineEvent.StartTime;
                existing.EndTime = timelineEvent.EndTime;
                database.SaveChanges();
            }
        }

        public void AddUserToEvent(TimelineEvent tevent, User user)
        {
            if (tevent == null || user == null) return;
            TimelineEvent existing = this.Get(tevent.TimelineEventId);
            if (existing != null)
            {
                if (existing.Users == null) { existing.Users = new List<User>(); }
                existing.Users.Add(user);
                database.SaveChanges();
            }
        }
        public void RemoveUserFromEvent(TimelineEvent tevent, User user)
        {
            if (tevent == null || user == null) return;
            TimelineEvent existing = this.Get(tevent.TimelineEventId);
            if (existing != null)
            {
                if (existing.Users == null) { return; }
                if (existing.Users.Contains(user))
                {
                    existing.Users.Remove(user);
                }
                database.SaveChanges();
            }
        }
    }
}

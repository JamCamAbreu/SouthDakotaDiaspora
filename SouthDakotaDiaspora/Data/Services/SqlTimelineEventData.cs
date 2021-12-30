using Data.Models;
using System;
using System.Collections.Generic;
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
            return database.TimelineEvents.Find(id);
        }

        public IEnumerable<TimelineEvent> GetAll()
        {
            return from t in database.TimelineEvents
                   orderby t.StartTime
                   select t;
        }

        public IEnumerable<TimelineEvent> GetPendingNotifySoonEvents()
        {
            List<TimelineEvent> items = database.TimelineEvents.ToList();
            return items.Where(tevent => tevent.StartTime <= DateTime.Now.AddHours(1) && tevent.EndTime > DateTime.Now && tevent.SentNotificationSoon == false);
        }
        public IEnumerable<TimelineEvent> GetPendingNotifyStartingEvents()
        {
            List<TimelineEvent> items = database.TimelineEvents.ToList();
            return items.Where(tevent => tevent.StartTime <= DateTime.Now && tevent.EndTime > DateTime.Now && tevent.SentNotificationStarting == false);
        }

        public IEnumerable<TimelineEvent> GetBeforeToday()
        {
            List<TimelineEvent> items = database.TimelineEvents.ToList();
            return items.Where(tevent => tevent.EndTime < DateTime.Now).OrderBy(tevent => tevent.StartTime);
        }
        public IEnumerable<TimelineEvent> GetToday()
        {
            DateTime endofday = DateTime.Now.Date.AddDays(1);
            List<TimelineEvent> items = database.TimelineEvents.ToList();
            return items.Where(tevent => tevent.EndTime >= DateTime.Now && tevent.EndTime < endofday).OrderBy(tevent => tevent.StartTime);
        }
        public IEnumerable<TimelineEvent> GetAfterToday()
        {
            DateTime endofday = DateTime.Now.Date.AddDays(1);
            List<TimelineEvent> items = database.TimelineEvents.ToList();
            return items.Where(tevent => tevent.StartTime > endofday).OrderBy(tevent => tevent.StartTime);
        }
        public void Update(TimelineEvent timelineEvent)
        {
            if (timelineEvent == null) return;
            TimelineEvent existing = this.Get(timelineEvent.TimelineEventId);
            if (existing != null)
            {
                existing.Host = timelineEvent.Host;
                existing.Title = timelineEvent.Title;
                existing.StartTime = timelineEvent.StartTime;
                existing.EndTime = timelineEvent.EndTime;
                existing.ActivityId = timelineEvent.ActivityId;
                database.SaveChanges();
            }
        }
    }
}

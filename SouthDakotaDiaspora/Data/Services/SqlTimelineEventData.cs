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

        public IEnumerable<TimelineEvent> GetAfterToday()
        {
            return from t in database.TimelineEvents
                   orderby t.StartTime
                   where t.StartTime > System.Data.Entity.DbFunctions.AddDays(DateTime.Today, 1)
                   select t;
        }

        public IEnumerable<TimelineEvent> GetAll()
        {
            return from t in database.TimelineEvents
                   orderby t.StartTime
                   select t;
        }

        public IEnumerable<TimelineEvent> GetBeforeToday()
        {
            return from t in database.TimelineEvents
                   orderby t.StartTime
                   where t.StartTime < DateTime.Now
                   select t;
        }

        public IEnumerable<TimelineEvent> GetToday()
        {
            return from t in database.TimelineEvents
                   orderby t.StartTime
                   where t.StartTime >= DateTime.Now && t.StartTime < System.Data.Entity.DbFunctions.AddDays(DateTime.Today, 1)
                   select t;
        }

        public void Update(TimelineEvent timelineEvent)
        {
            if (timelineEvent == null) return;
            TimelineEvent existing = this.Get(timelineEvent.Id);
            if (existing != null)
            {
                existing.Host = timelineEvent.Host;
                existing.Title = timelineEvent.Title;
                existing.StartTime = timelineEvent.StartTime;
                existing.EndTime = timelineEvent.EndTime;
                existing.ActivityId = timelineEvent.ActivityId;
            }
        }
    }
}

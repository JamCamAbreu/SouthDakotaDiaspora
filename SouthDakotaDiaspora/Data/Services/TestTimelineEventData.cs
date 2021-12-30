using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class TestTimelineEventData : ITimelineEventData
    {
        List<TimelineEvent> timelineEvents;
        public TestTimelineEventData()
        {
            this.timelineEvents = new List<TimelineEvent>()
            {
                new TimelineEvent() {
                    TimelineEventId = 1,
                    Title = "Test Event 1",
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2)},

                new TimelineEvent() {
                    TimelineEventId = 2,
                    Title = "Test Event 2",
                    StartTime = DateTime.Now.AddHours(24),
                    EndTime = DateTime.Now.AddHours(27)},

                new TimelineEvent() {
                    TimelineEventId = 3,
                    Title = "Test Event 3",
                    StartTime = DateTime.Now.AddHours(25),
                    EndTime = DateTime.Now.AddHours(26)},

                new TimelineEvent() {
                    TimelineEventId = 4,
                    Title = "Test Event 4",
                    StartTime = DateTime.Now.AddHours(42),
                    EndTime = DateTime.Now.AddHours(43)},
            };
        }

        public void Add(TimelineEvent timelineEvent)
        {
            timelineEvent.TimelineEventId = this.timelineEvents.Max(t => t.TimelineEventId) + 1;
            this.timelineEvents.Add(timelineEvent);
        }

        public void Delete(int id)
        {
            TimelineEvent timelineEvent = this.Get(id);
            if (timelineEvent != null)
            {
                this.timelineEvents.Remove(timelineEvent);
            }
        }

        public TimelineEvent Get(int id)
        {
            return this.timelineEvents.FirstOrDefault(t => t.TimelineEventId == id);
        }

        public IEnumerable<TimelineEvent> GetAfterToday()
        {
            return this.timelineEvents.Where(t => t.StartTime >= DateTime.Today.AddDays(1));
        }

        public IEnumerable<TimelineEvent> GetAll()
        {
            return timelineEvents;
        }

        public IEnumerable<TimelineEvent> GetBeforeToday()
        {
            return this.timelineEvents.Where(t => t.StartTime < DateTime.Today);
        }

        public IEnumerable<TimelineEvent> GetPendingNotifySoonEvents()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TimelineEvent> GetPendingNotifyStartingEvents()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TimelineEvent> GetToday()
        {
            return this.timelineEvents.Where(t => t.StartTime >= DateTime.Today && t.StartTime < DateTime.Today.AddDays(1));
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
            }
        }
    }
}

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
        IEnumerable<TimelineEvent> timelineEvents;
        public TestTimelineEventData()
        {
            this.timelineEvents = new List<TimelineEvent>()
            {
                new TimelineEvent() {
                    Id = 1,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2),
                    EventType = TimelineEventType.Game},

                new TimelineEvent() {
                    Id = 2,
                    StartTime = DateTime.Now.AddHours(24),
                    EndTime = DateTime.Now.AddHours(27),
                    EventType = TimelineEventType.Game},

                new TimelineEvent() {
                    Id = 3,
                    StartTime = DateTime.Now.AddHours(25),
                    EndTime = DateTime.Now.AddHours(26),
                    EventType = TimelineEventType.Show},

                new TimelineEvent() {
                    Id = 4,
                    StartTime = DateTime.Now.AddHours(42),
                    EndTime = DateTime.Now.AddHours(43),
                    EventType = TimelineEventType.Game},
            };
        }
        public IEnumerable<TimelineEvent> GetAll()
        {
            return timelineEvents;
        }
    }
}

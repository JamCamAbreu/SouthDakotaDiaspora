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
                    Title = "Test Event 1",
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2)},

                new TimelineEvent() {
                    Id = 2,
                    Title = "Test Event 2",
                    StartTime = DateTime.Now.AddHours(24),
                    EndTime = DateTime.Now.AddHours(27)},

                new TimelineEvent() {
                    Id = 3,
                    Title = "Test Event 3",
                    StartTime = DateTime.Now.AddHours(25),
                    EndTime = DateTime.Now.AddHours(26)},

                new TimelineEvent() {
                    Id = 4,
                    Title = "Test Event 4",
                    StartTime = DateTime.Now.AddHours(42),
                    EndTime = DateTime.Now.AddHours(43)},
            };
        }
        public IEnumerable<TimelineEvent> GetAll()
        {
            return timelineEvents;
        }
    }
}

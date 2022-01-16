using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface ITimelineEventData
    {
        void Add(TimelineEvent timelineEvent);
        void Delete(int id);
        void Update(TimelineEvent timelineEvent);
        void AddUserToEvent(TimelineEvent tevent, User user);
        void RemoveUserFromEvent(TimelineEvent tevent, User user);
        TimelineEvent Get(int id);
        IEnumerable<TimelineEvent> GetAll();
        IEnumerable<TimelineEvent> GetBeforeToday();
        IEnumerable<TimelineEvent> GetToday();
        IEnumerable<TimelineEvent> GetAfterToday();
        IEnumerable<TimelineEvent> GetPendingNotifySoonEvents();
        IEnumerable<TimelineEvent> GetPendingNotifyStartingEvents();
    }
}

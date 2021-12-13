using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class TimelineEvent
    {
        public int Id { get; set; }
        public TimelineEventType EventType { get; set; }
        public int MediaID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public User Host { get; set; }
        public List<User> UsersGoing { get; set; }
        public List<User> UsersMaybe { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class TimelineEvent
    {
        public TimelineEvent() 
        {
            this.Users = new List<User>();
            this.Comments = new List<Comment>();
        }
        public TimelineEvent(User host)
        {
            this.Users = new List<User>() { host };
            this.Host = host;
            this.Comments = new List<Comment>();
        }

        [Key]
        public int TimelineEventId { get; set; }
        [MaxLength(128)]
        public string Title { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
        public User Host { get; set; }
        [Display(Name = "Users Going")]
        public ICollection<User> Users { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public bool SentNotificationStarting { get; set; }
        public bool SentNotificationSoon { get; set; }
    }
}

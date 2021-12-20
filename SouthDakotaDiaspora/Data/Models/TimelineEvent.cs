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
        public int Id { get; set; }
        [MaxLength(128)]
        public string Title { get; set; }
        public Activity ActivityId { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:ddd (MM/dd/yyyy) h:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:ddd (MM/dd/yyyy) h:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
        public User Host { get; set; }
        [Display(Name = "Users Going")]
        public List<User> UsersGoing { get; set; }
        [Display(Name = "Users Maybe")]
        public List<User> UsersMaybe { get; set; }
        public List<Comment> Comments { get; set; }
    }
}

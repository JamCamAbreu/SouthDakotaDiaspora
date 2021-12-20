using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models.Timeline
{
    public class TimelineIndexViewModel
    {
        public TimelineIndexViewModel()
        {
            Scaffolding = new TimelineEvent();
        }
        public TimelineEvent Scaffolding { get; set; }
        public List<TimelineEvent> PastEvents { get; set; }
        public List<TimelineEvent> TodayEvents { get; set; }
        public List<TimelineEvent> FutureEvents { get; set; }
    }
}
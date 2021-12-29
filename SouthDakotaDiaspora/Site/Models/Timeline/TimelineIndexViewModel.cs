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
            Scaffolding = new TimelineEventInfo();

            PastEvents = new List<TimelineEventInfo>();
            TodayEvents = new List<TimelineEventInfo>();
            FutureEvents = new List<TimelineEventInfo>();

            this.MorePastEvents = false;
            this.MoreFutureEvents = false;
        }
        public TimelineEventInfo Scaffolding { get; set; }
        public string TimeZoneName { get; set; }

        public List<TimelineEventInfo> PastEvents { get; set; }
        public bool MorePastEvents { get; set; }

        public List<TimelineEventInfo> TodayEvents { get; set; }

        public List<TimelineEventInfo> FutureEvents { get; set; }
        public bool MoreFutureEvents { get; set; }
    }
}
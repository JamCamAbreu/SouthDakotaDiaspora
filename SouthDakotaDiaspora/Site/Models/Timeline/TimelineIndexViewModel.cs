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
            Scaffolding = new TimelineRow();

            PastEvents = new List<TimelineRow>();
            TodayEvents = new List<TimelineRow>();
            FutureEvents = new List<TimelineRow>();

            this.MorePastEvents = false;
            this.MoreFutureEvents = false;
        }
        public TimelineRow Scaffolding { get; set; }
        public string TimeZoneName { get; set; }

        public List<TimelineRow> PastEvents { get; set; }
        public bool MorePastEvents { get; set; }

        public List<TimelineRow> TodayEvents { get; set; }

        public List<TimelineRow> FutureEvents { get; set; }
        public bool MoreFutureEvents { get; set; }
    }
}
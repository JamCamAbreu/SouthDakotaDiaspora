using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Models.Timeline
{
    public class TimelineCreateViewModel
    {
        public TimelineEvent Tevent { get; set; }
        public List<SelectListItem> GameSelection { get; set; }
        public List<SelectListItem> ShowSelection { get; set; }
        public List<SelectListItem> BookSelection { get; set; }
        public List<SelectListItem> ProjectSelection { get; set; }

        public TimelineCreateViewModel(TimelineEvent tevent)
        {
            this.Tevent = tevent;
        }

        public static List<SelectListItem> ListToDropdown(List<Activity> activityList)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Activity activity in activityList)
            {
                items.Add(new SelectListItem() { Text = activity.Name, Value = activity.ActivityId.ToString() });
            }
            return items;
        }
    }
}
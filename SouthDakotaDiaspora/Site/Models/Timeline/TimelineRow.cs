using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Site.Models.Timeline
{
    public class TimelineRow
    {
        const string TIME_FORMAT = "n2";
        public DateTime StartTime { get; set; }
        public string StartTimeDisplay { get; set; }
        public string Duration { get; set; }
        public string Platform { get; set; }
        public string ActivityDisplay { get; set; }
        public string Attending { get; set; }
        public int Id { get; set; }
        public TimelineRow () { }
        public TimelineRow(TimelineEvent tevent, TimeZoneInfo localtimezone)
        {
            this.Id = tevent.Id;

            DateTime localizedStartTime = TimeZoneInfo.ConvertTime(tevent.StartTime, localtimezone);
            DateTime localizedEndTime = TimeZoneInfo.ConvertTime(tevent.EndTime, localtimezone);
            this.StartTime = localizedStartTime;
            this.StartTimeDisplay = this.CleanStartTime(localizedStartTime, localizedEndTime);
            
            this.Duration = this.CleanDuration(tevent.StartTime, tevent.EndTime);

            this.Platform = tevent.Title; // Todo
            this.ActivityDisplay = tevent.Title; // Todo

            if (tevent.UsersGoing != null && tevent.UsersGoing.Count > 0)
            {
                string userFirstNames = string.Join(", ", tevent.UsersGoing.Select(u => u.FirstName));
                this.Attending = userFirstNames;
            }

        }

        protected string CleanStartTime(DateTime starttime, DateTime endtime)
        {
            // Future
            if (starttime >= DateTime.Now)
            {
                TimeSpan timeuntil = starttime - DateTime.Now;
                if (timeuntil.TotalMinutes < 5)
                {
                    return $"Starting now! Hop on discord!";
                }
                else if (timeuntil.TotalMinutes < 60)
                {
                    return $"In {(int)Math.Round(timeuntil.TotalMinutes)} minutes ({starttime.ToString("h:mm tt")})";
                }
                else if (timeuntil.TotalHours < 24)
                {
                    return $"In {(int)Math.Round(timeuntil.TotalHours)} hours ({starttime.ToString("h:mm tt")})";
                }
                else if (starttime > DateTime.Today.AddDays(1) && starttime < DateTime.Today.AddDays(2))
                {
                    return $"Tomorrow ({starttime.ToString("h:mm tt")})";
                }
                else
                {
                    return $"In {(int)Math.Round(timeuntil.TotalDays)} days ({starttime.ToString("h:mm tt")})";
                }
            }

            // Past
            else
            {
                TimeSpan timeago = DateTime.Now - starttime;

                // Event still going on
                if (endtime > DateTime.Now)
                {
                    return $"started {(int)Math.Round(timeago.TotalMinutes)} minutes ago";
                }
                
                // Event Ended some time today:
                else if (timeago.TotalMinutes < 10)
                {
                    return "A few minutes ago";
                }
                else if (timeago.TotalMinutes < 60)
                {
                    return $"{(int)Math.Round(timeago.TotalMinutes)} minutes ago";
                }
                else if (timeago.TotalHours < 24)
                {
                    return $"{(int)Math.Round(timeago.TotalHours)} hours ago";
                }
                else
                {
                    return $"{(int)Math.Round(timeago.TotalDays)} days ago";
                }
            }
        }
        protected string CleanDuration(DateTime starttime, DateTime endtime)
        {
            TimeSpan duration = endtime - starttime;
            if (duration.TotalMinutes < 60)
            {
                return $"{(int)Math.Round(duration.TotalMinutes)} minutes";
            }
            else if (duration.TotalHours < 24)
            {
                return $"{(int)Math.Round(duration.TotalHours)} hours";
            }
            else
            {
                return $"{(int)Math.Round(duration.TotalDays)} days";
            }
        }
    }
}
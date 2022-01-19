using Data.Models;
using Site.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Site.Models.Timeline
{
    public class TimelineEventInfo
    {
        public DateTime StartTime { get; set; }
        public string StartTimeDisplay { get; set; }
        public string Duration { get; set; }
        public string Platform { get; set; }
        public string PlatformAbbreviation { get; set; }
        public string ActivityName { get; set; }
        public string ActivityNameAbbreviated { get; set; }
        public int ActivityReferenceId { get; set; }
        public string ActivityType { get; set; }
        public string Title { get; set; }
        public string TitleAbbreviated { get; set; }
        public string Type { get; set; }
        public string Attending { get; set; }
        public List<int> AttendingIds { get; set; }
        public string Host { get; set; }
        public int? HostId { get; set; }
        public int TimelineEventId { get; set; }
        public int MaxAttendees { get; set; }
        public string MaxAttendeesDisplay { get; set; }
        public TimelineEventInfo () { }
        public TimelineEventInfo(TimelineEvent tevent, TimeZoneInfo localtimezone, Activity activityinfo)
        {
            this.TimelineEventId = tevent.TimelineEventId;

            DateTime localizedStartTime = TimeZoneInfo.ConvertTime(tevent.StartTime, localtimezone);
            DateTime localizedEndTime = TimeZoneInfo.ConvertTime(tevent.EndTime, localtimezone);
            this.StartTime = localizedStartTime;
            this.StartTimeDisplay = this.CleanStartTime(localizedStartTime, localizedEndTime, localtimezone);

            this.Title = tevent.Title;
            this.TitleAbbreviated = GlobalMethods.Abbreviate(tevent.Title, GlobalMethods.MAX_CHARACTERS_MEDIUM);
            
            this.Duration = this.CleanDuration(tevent.StartTime, tevent.EndTime);

            if (activityinfo != null)
            {
                this.Platform = activityinfo.Platform.ToString();
                this.PlatformAbbreviation = GlobalMethods.Abbreviate(activityinfo.Platform.ToString(), GlobalMethods.MAX_CHARACTERS_MEDIUM);

                this.ActivityName = activityinfo.Name;
                this.ActivityNameAbbreviated = GlobalMethods.Abbreviate(activityinfo.Name, GlobalMethods.MAX_CHARACTERS_MEDIUM);
                this.ActivityReferenceId = activityinfo.ActivityId;
                this.ActivityType = activityinfo.ActivityType;
            }

            if (tevent.Host != null)
            {
                this.Host = GlobalMethods.AbbreviateName(tevent.Host.FirstName, tevent.Host.LastName);
                this.HostId = tevent.Host.UserId;
            }

            this.AttendingIds = new List<int>();
            if (tevent.Users != null && tevent.Users.Count > 0)
            {
                List<User> otherusers = tevent.Users.ToList();
                if (tevent.Host != null)
                {
                    otherusers.Remove(tevent.Host);
                }

                string userFirstNames = string.Join(", ", otherusers.Select(u => GlobalMethods.AbbreviateName(u.FirstName, u.LastName)));
                this.Attending = userFirstNames;

                foreach (User user in tevent.Users)
                {
                    this.AttendingIds.Add(user.UserId);
                }
            }

            this.Type = GetTypeSymbol(activityinfo.ActivityType);
            this.MaxAttendees = tevent.MaxAttendees;
            if (tevent.MaxAttendees > 0)
            {
                this.MaxAttendeesDisplay = tevent.MaxAttendees.ToString();
            }
            else
            {
                this.MaxAttendeesDisplay = "Unlimited";
            }
        }

        protected string GetTypeSymbol(string activityType)
        {
            if (activityType == Game.ACTIVITY_TYPE)
            {
                return "🎲";
            }
            else if (activityType == Show.ACTIVITY_TYPE)
            {
                return "📺";
            }
            else if (activityType == Book.ACTIVITY_TYPE)
            {
                return "📚";
            }
            else if (activityType == Project.ACTIVITY_TYPE)
            {
                return "🧪";
            }
            else
            {
                return "?";
            }
        }
        protected string CleanStartTime(DateTime starttime, DateTime endtime, TimeZoneInfo localTimeZone)
        {
            // Future

            DateTime nowLocalized = TimeZoneInfo.ConvertTime(DateTime.Now, localTimeZone);

            if (starttime >= nowLocalized)
            {
                TimeSpan timeuntil = starttime - nowLocalized;
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
                TimeSpan timeago = nowLocalized - starttime;

                // Event still going on
                if (endtime > nowLocalized)
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
                return $"{(float)Math.Round(duration.TotalMinutes, 1)} minutes";
            }
            else if (duration.TotalHours < 24)
            {
                return $"{(float)Math.Round(duration.TotalHours, 1)} hours";
            }
            else
            {
                return $"{(float)Math.Round(duration.TotalDays, 1)} days";
            }
        }
    }
}
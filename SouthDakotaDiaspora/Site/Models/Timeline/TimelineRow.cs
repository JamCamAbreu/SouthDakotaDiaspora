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
        const int MAX_CHARACTERS = 16;
        public DateTime StartTime { get; set; }
        public string StartTimeDisplay { get; set; }
        public string Duration { get; set; }
        public string Platform { get; set; }
        public string PlatformAbbreviation { get; set; }
        public string ActivityName { get; set; }
        public string ActivityNameAbbreviated { get; set; }
        public string Title { get; set; }
        public string TitleAbbreviated { get; set; }
        public string Type { get; set; }
        public string Attending { get; set; }
        public int Id { get; set; }
        public TimelineRow () { }
        public TimelineRow(TimelineEvent tevent, TimeZoneInfo localtimezone, Activity activityinfo)
        {
            this.Id = tevent.Id;

            DateTime localizedStartTime = TimeZoneInfo.ConvertTime(tevent.StartTime, localtimezone);
            DateTime localizedEndTime = TimeZoneInfo.ConvertTime(tevent.EndTime, localtimezone);
            this.StartTime = localizedStartTime;
            this.StartTimeDisplay = this.CleanStartTime(localizedStartTime, localizedEndTime);

            this.Title = tevent.Title;
            this.TitleAbbreviated = this.Abbreviate(tevent.Title, MAX_CHARACTERS);
            
            this.Duration = this.CleanDuration(tevent.StartTime, tevent.EndTime);

            if (activityinfo != null)
            {
                this.Platform = activityinfo.Platform.ToString();
                this.PlatformAbbreviation = this.Abbreviate(activityinfo.Platform.ToString(), MAX_CHARACTERS);

                this.ActivityName = activityinfo.Name;
                this.ActivityNameAbbreviated = this.Abbreviate(activityinfo.Name, MAX_CHARACTERS);
            }

            if (tevent.UsersGoing != null && tevent.UsersGoing.Count > 0)
            {
                string userFirstNames = string.Join(", ", tevent.UsersGoing.Select(u => u.FirstName));
                this.Attending = userFirstNames;
            }

            this.Type = GetTypeSymbol(null);

        }
        protected string Abbreviate(string word, int maxChar)
        {
            if (!string.IsNullOrEmpty(word) && word.Length > maxChar)
            {
                return word.Substring(0, maxChar) + "...";
            }
            else
            {
                return word;
            }
        }
        protected string GetTypeSymbol(Type activityType)
        {
            if (activityType == typeof(Game))
            {
                return "🎲";
            }
            else if (activityType == typeof(Show))
            {
                return "📺";
            }
            else if (activityType == typeof(Book))
            {
                return "📚";
            }
            else if (activityType == typeof(Project))
            {
                return "🧪";
            }
            else
            {
                return "?";
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
﻿using Data.Models;
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
        public int TimelineEventId { get; set; }
        public TimelineEventInfo () { }
        public TimelineEventInfo(TimelineEvent tevent, TimeZoneInfo localtimezone, Activity activityinfo)
        {
            this.TimelineEventId = tevent.TimelineEventId;

            DateTime localizedStartTime = TimeZoneInfo.ConvertTime(tevent.StartTime, localtimezone);
            DateTime localizedEndTime = TimeZoneInfo.ConvertTime(tevent.EndTime, localtimezone);
            this.StartTime = localizedStartTime;
            this.StartTimeDisplay = this.CleanStartTime(localizedStartTime, localizedEndTime);

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

            if (tevent.UsersGoing != null && tevent.UsersGoing.Count > 0)
            {
                string userFirstNames = string.Join(", ", tevent.UsersGoing.Select(u => u.FirstName));
                this.Attending = userFirstNames;
            }

            this.Type = GetTypeSymbol(activityinfo.ActivityType);

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
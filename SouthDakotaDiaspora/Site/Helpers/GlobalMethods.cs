using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Helpers
{
    public static class GlobalMethods
    {
        public static bool IsLoggedIn(HttpSessionStateBase session)
        {
            if (session == null || session["UserID"] == null) return false;
            return true;
        }
        public static int? GetCurrentUserId(HttpSessionStateBase session)
        {
            if(IsLoggedIn(session) && session["UserID"] != null)
            {
                int userid;
                if (int.TryParse(session["UserID"].ToString(), out userid))
                {
                    return userid;
                }
            }
            return null;
        }
        public static void UpdateSession(HttpSessionStateBase session, User user)
        {
            if (user == null || session == null)
            {
                return;
            }

            if (session["UserName"] == null || session["UserName"].ToString() != user.Username)
            {
                session["UserName"] = user.Username;
            }
            if (session["UserID"] == null || session["UserID"].ToString() != user.UserId.ToString())
            {
                session["UserID"] = user.UserId;
            }
            if (session["Role"] == null || session["Role"].ToString() != user.UserRole.ToString())
            {
                session["Role"] = user.UserRole.ToString();
            }

            TimeZoneInfo tzi = TimeZoneInfo.Local;
            string timezonecode = "-420"; // mountain
            switch (user.TimeZonePreference)
            {
                case UserTimeZoneType.PacificStandardTime:
                    tzi = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    timezonecode = "-480";
                    break;

                case UserTimeZoneType.MountainStandardTime:
                    tzi = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
                    timezonecode = "-420";
                    break;

                case UserTimeZoneType.CentralStandardTime:
                    tzi = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                    timezonecode = "-360";
                    break;

                case UserTimeZoneType.EasternStandardTime:
                    tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    timezonecode = "-300";
                    break;
            }
            session["TimeZoneInfo"] = tzi;
            session["TimeZoneCode"] = timezonecode;
        }
        public static bool IsAdmin(HttpSessionStateBase session)
        {
            if (session["Role"] != null && session["Role"].ToString() == Data.Models.UserRoleType.Admin.ToString()) { return true; }
            return false;
        }
        public static bool IsOnMyUserDetailsPage(HttpSessionStateBase session, ViewContext context)
        {
            if(Site.Helpers.GlobalMethods.IsLoggedIn(session) &&
                !string.IsNullOrEmpty(session["UserID"].ToString()) &&
                context.RouteData.Values["controller"]?.ToString() == "Users" &&
                context.RouteData.Values["id"]?.ToString() == session["UserID"].ToString())
            {
                return true;         
            }

            return false;

        }
        public static void ClearSession(HttpSessionStateBase session)
        {
            if (session == null) return;
            if (session["UserID"] != null)
            {
                session.Remove("UserID");
            }
            if (session["UserName"] != null)
            {
                session.Remove("UserName");
            }
            if (session["Role"] != null)
            {
                session.Remove("Role");
            }
            if (session["TimeZoneInfo"] != null)
            {
                session.Remove("TimeZoneInfo");
            }
            if (session["TimeZoneCode"] != null)
            {
                session.Remove("TimeZoneCode");
            }
        }
        public static void AddConfirmationMessage(HttpSessionStateBase session, string message)
        {
            if (session == null || string.IsNullOrEmpty(message))
            {
                return;
            }

            if (session["ConfirmationMessages"] == null)
            {
                session["ConfirmationMessages"] = new Queue<string>();
            }
            Queue<string> messages = session["ConfirmationMessages"] as Queue<string>;
            messages.Enqueue(message);
        }
        public static int NumberConfirmationMessages(HttpSessionStateBase session)
        {
            if (session == null) return 0;
            if (session["ConfirmationMessages"] == null)
            {
                return 0;
            }
            Queue<string> messages = session["ConfirmationMessages"] as Queue<string>;
            return messages.Count;
        }
        public static String DequeueConfirmationMessage(HttpSessionStateBase session)
        {
            if (session == null) return null;
            if (session["ConfirmationMessages"] == null)
            {
                return null;
            }
            Queue<string> messages = session["ConfirmationMessages"] as Queue<string>;
            if (messages.Count == 0) return null;
            else return messages.Dequeue();
        }

        public const int MAX_CHARACTERS_SMALL = 8;
        public const int MAX_CHARACTERS_MEDIUM = 16;
        public const int MAX_CHARACTERS_LARGE = 32;
        public const int MAX_CHARACTERS_HUGE = 64;
        public static string Abbreviate(string word, int maxChars)
        {
            if (!string.IsNullOrEmpty(word) && word.Length > maxChars)
            {
                return word.Substring(0, maxChars) + "...";
            }
            else
            {
                return word;
            }
        }

        public static string AbbreviateName(string firstname, string lastname)
        {
            string lastinitial = !string.IsNullOrEmpty(lastname) ? lastname.Substring(0, 1) : "";
            return $"{firstname} {lastinitial}.";
        }

        public static string FormatTime(DateTime? timenullable)
        {
            if (timenullable == null) return "";
            DateTime time = (DateTime)timenullable;

            // Future
            if (time >= DateTime.Now)
            {
                TimeSpan timeuntil = time - DateTime.Now;
                if (timeuntil.TotalMinutes < 5)
                {
                    return $"In a few minutes";
                }
                else if (timeuntil.TotalMinutes < 60)
                {
                    return $"In {(int)Math.Round(timeuntil.TotalMinutes)} minutes ({time.ToString("h:mm tt")})";
                }
                else if (timeuntil.TotalHours < 24)
                {
                    return $"In {(int)Math.Round(timeuntil.TotalHours)} hours ({time.ToString("h:mm tt")})";
                }
                else if (time > DateTime.Today.AddDays(1) && time < DateTime.Today.AddDays(2))
                {
                    return $"Tomorrow ({time.ToString("h:mm tt")})";
                }
                else
                {
                    return $"In {(int)Math.Round(timeuntil.TotalDays)} days ({time.ToString("h:mm tt")})";
                }
            }

            // Past
            else
            {
                TimeSpan timeago = DateTime.Now - time;

                // Event Ended some time today:
                if (timeago.TotalMinutes < 10)
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
    }
}
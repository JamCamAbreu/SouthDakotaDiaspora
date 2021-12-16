using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Helpers
{
    public static class GlobalMethods
    {
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
            if (session["UserID"] == null || session["UserID"].ToString() != user.Id.ToString())
            {
                session["UserID"] = user.Id;
            }
            if (session["Role"] == null || session["Role"].ToString() != user.UserRole.ToString())
            {
                session["Role"] = user.UserRole.ToString();
            }
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
    }
}
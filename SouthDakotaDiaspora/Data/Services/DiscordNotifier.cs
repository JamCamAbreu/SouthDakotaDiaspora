using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Data.Models;
using Discord.Webhook;

namespace Data.Services
{
    public class DiscordNotifier
    {
        private string[] hookMessages;
        private static Random random;
        private ITimelineEventData database;
        DiscordWebhookClient client;
        public DiscordNotifier(ITimelineEventData db)
        {
            this.database = db;
            this.hookMessages = new string[] 
            {
                "Don't miss out! 😱",
                "Would you dare miss it?! 😮",
                "Cancel all your plans! ✔",
                "Get ready to bring the storm. 🌩",
                "Grab your pals! 🍻",
                "Hold on to your butts! 🧈",
                "Can you believe it?! 😲",
                "Who will come out on top?! 🏆",
                "Prepare the hatches! ⚒",
                "Sound the horn! 📯",
                "Hurry! Mount the ramparts! 🏰",
                "To the battlements! 🏹",
                "Quickly! Lower the portcullis and man your stations! 🛡",
                "Protect the women and children! ⚔",
                "Don't forget your ales! 🍺"
            };
            random = new Random();

            this.client = new DiscordWebhookClient("https://discord.com/api/webhooks/923641775627858020/SZdlb-4qURdI9cmeULArEmvaW_xy-wYS_mIQyNoKyZqVEeoMJCOWRoq5JUmx9VYKt5gO");
        }

        ~DiscordNotifier()
        {
            this.client.Dispose();
        }

        public async Task SendSoonNotifications()
        {
            TimelineEvent[] notifySoonEvents = this.database.GetPendingNotifySoonEvents().ToArray();
            foreach (TimelineEvent tevent in notifySoonEvents)
            {
                string hook = this.hookMessages[random.Next(0, this.hookMessages.Length)];
                string message = $"{tevent.Title} is starting within the hour! ({tevent.StartTime.ToShortTimeString()} MST) - {hook}";
                Debug.WriteLine(message);

                //await this.client.SendMessageAsync(message);

                // Todo: When I get a permanent URL, add a "find more details here" link that uses the tevent's id. 

                tevent.SentNotificationSoon = true;
                this.database.Update(tevent);
            }
        }

        public async Task SendStartingNotifications()
        {
            TimelineEvent[] notifyStartingEvents = this.database.GetPendingNotifyStartingEvents().ToArray();
            foreach (TimelineEvent tevent in notifyStartingEvents)
            {
                string hook = this.hookMessages[random.Next(0, this.hookMessages.Length)];
                string message = $"{tevent.Title} has started!";
                Debug.WriteLine(message);

                //await this.client.SendMessageAsync(message);

                // Todo: When I get a permanent URL, add a "find more details here" link that uses the tevent's id. 

                tevent.SentNotificationStarting = true;
                this.database.Update(tevent);
            }
        }

    }
}

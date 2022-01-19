using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Data.Models;
using Discord.Webhook;
using System.Configuration;

namespace Data.Services
{
    public class DiscordNotifier
    {
        private string[] hookMessages;
        private string[] personifications;
        private static Random random;
        private ITimelineEventData database;
        DiscordWebhookClient client;
        public DiscordNotifier(ITimelineEventData db)
        {
            this.database = db;

            this.personifications = new string[]
            {
                "peasants",
                "challengers",
                "contestors",
                "creatures",
                "jesters",
                "willful souls",
                "minds",
                "beings",
                "would be champions",
                "counsel"
            };

            this.hookMessages = new string[] 
            {
                "Forests will fall...",
                "So..you have chosen death? 🧙‍",
                "The hour is later than you think. Sauron’s forces are already moving.",
                "They will find the Ring… and kill the one who carries it.",
                "You did not seriously think that a hobbit could contend with the will of Sauron? There are none who can.",
                "Against the power of Mordor there can be no victory.",
                "We must join with Sauron. It would be wise, my friend.",
                "I gave you the chance of aiding me willingly. But you… have elected… the way of… pain!",
                "He is gathering all evil to him. Very soon he will summon an army great enough to launch an assault upon Middle-Earth.",
                "Concealed within his fortress, the Lord of Mordor sees all",
                "Time?! What time do you think we have?",
                "Smoke rises from the mountain of Doom.",
                "Tens of thousands.",
                "If the wall is breached, Helm’s Deep will fall.",
                "Gandalf the White. Gandalf the Fool! Does he seek to humble me with his newfound piety?",
                "Your love of the Halfings’ leaf has clearly slowed your mind."
            };
            random = new Random();

            this.client = new DiscordWebhookClient(ConfigurationManager.AppSettings["DiscordWebHook"]);
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
                await this.client.SendMessageAsync("Behold!");

                string message = $"{GetEventMessage(tevent)} will begin within the hour. ({tevent.StartTime.ToShortTimeString()} MST)\n";
                await this.client.SendMessageAsync(message);

                await SendOthersIncluded(tevent);

                message = $"Check it out here: http://abreu.wiki/Timeline";
                await this.client.SendMessageAsync(message);

                string hook = this.hookMessages[random.Next(0, this.hookMessages.Length)];
                await this.client.SendMessageAsync(hook);

                tevent.SentNotificationSoon = true;
                this.database.Update(tevent);

            }
        }

        public async Task SendStartingNotifications()
        {
            TimelineEvent[] notifyStartingEvents = this.database.GetPendingNotifyStartingEvents().ToArray();
            foreach (TimelineEvent tevent in notifyStartingEvents)
            {
                await this.client.SendMessageAsync("Alas!");

                string message = $"{GetEventMessage(tevent)} has begun! At last!";
                await this.client.SendMessageAsync(message);

                await SendOthersIncluded(tevent);

                tevent.SentNotificationStarting = true;
                this.database.Update(tevent);
            }
        }

        public string GetEventMessage(TimelineEvent tevent)
        {
            string hostcallout = "";
            if (tevent.Host != null && !string.IsNullOrEmpty(tevent.Host.DiscordId)) { hostcallout = $"(<@{tevent.Host.DiscordId}>)"; }
            string platform = string.IsNullOrEmpty(tevent.Activity?.Platform.ToString()) ? $"- {tevent.Activity.Platform}" : "";
            string message = $"{tevent.Title} ({tevent.Activity.Name}{platform})\nHosted by {AbbreviateName(tevent.Host.FirstName, tevent.Host.LastName)} {hostcallout}";
            return message;
        }

        public async Task SendOthersIncluded(TimelineEvent tevent)
        {
            if (tevent.Host != null && tevent.Users != null && tevent.Users.Count > 1)
            {
                List<User> otherusers = tevent.Users.Except(tevent.Users.Where(u => u.UserId == tevent.Host.UserId)).ToList();
                string personification = this.personifications[random.Next(0, this.personifications.Length)];
                string message = $"Other {personification} in attendance: ";
                message += string.Join(", ", otherusers.Select(u => AbbreviateName(u.FirstName, u.LastName) + (!string.IsNullOrEmpty(u.DiscordId) ? $" (<@{u.DiscordId}>)" : "")));
                await this.client.SendMessageAsync(message);
            }
        }

        public static string AbbreviateName(string firstname, string lastname)
        {
            string lastinitial = !string.IsNullOrEmpty(lastname) ? lastname.Substring(0, 1) : "";
            return $"{firstname} {lastinitial}.";
        }


        public async Task SendTodaysLineupNotification()
        {
            TimelineEvent[] notifyTodayEvents = this.database.GetToday().ToArray();

            if (notifyTodayEvents.Length > 0)
            {
                StringBuilder message = new StringBuilder();
                message.Append("I now bring to you today's forecast of events:\n```json\n");
                foreach (TimelineEvent tevent in notifyTodayEvents)
                {
                    message.Append($"{tevent.Activity.Name} at {tevent.StartTime.ToShortTimeString()} (MST)\n");
                }
                message.Append("\n```\nGandalf thinks it unwise to use the Palantír...but Gandalf is a fool!");
                await this.client.SendMessageAsync(message.ToString());
            }
        }

        public async Task SendCreatedEvent(TimelineEvent tevent)
        {
            if (tevent == null || tevent.Activity == null || tevent.Host == null)
            {
                return;
            }
            StringBuilder message = new StringBuilder();
            message.Append("Prithee, listen well!\n");
            message.Append($"{tevent.Activity.Name} - ({tevent.Title}) hosted by {tevent.Host.FirstName} will begin on {tevent.StartTime.ToShortDateString()} {tevent.StartTime.ToShortTimeString()} (MST)\n");

            // check if activity has spots left (could be only 1 player / stream):
            message.Append($"Signups are available now, pray thee earn a spot or be left for the Uruk-hai! (http://abreu.wiki/timeline)");
            await this.client.SendMessageAsync(message.ToString());
        }
    }
}

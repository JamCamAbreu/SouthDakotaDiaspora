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

            this.client = new DiscordWebhookClient("https://discord.com/api/webhooks/926261703400915034/H1U_VxE3DMH1LGqj2ur4jUfMpWE_VXVAu-OaTf-L0OZBiolsawZ8_hEsAROIZnK-Y7wD");
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
                
                string message = $"{tevent.Title} hosted by {AbbreviateName(tevent.Host.FirstName, tevent.Host.LastName)} will begin within the hour. ({tevent.StartTime.ToShortTimeString()} MST)";
                await this.client.SendMessageAsync(message);

                if (tevent.Users != null && tevent.Users.Count > 0)
                {
                    string personification = this.personifications[random.Next(0, this.personifications.Length)];
                    message = $"Other {personification} in attendance: ";
                    message += string.Join(", ", tevent.Users.Select(u => AbbreviateName(u.FirstName, u.LastName)));
                    await this.client.SendMessageAsync(message);
                }

                string hook = this.hookMessages[random.Next(0, this.hookMessages.Length)];
                await this.client.SendMessageAsync(hook);

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
                string message = $"{tevent.Title} hosted by {AbbreviateName(tevent.Host.FirstName, tevent.Host.LastName)} has begun! At last!";
                await this.client.SendMessageAsync(message);

                if (tevent.Users != null && tevent.Users.Count > 0)
                {
                    string personification = this.personifications[random.Next(0, this.personifications.Length)];
                    message = $"Other {personification} in attendance: ";
                    message += string.Join(", ", tevent.Users.Select(u => AbbreviateName(u.FirstName, u.LastName)));
                    await this.client.SendMessageAsync(message);
                }

                // Todo: When I get a permanent URL, add a "find more details here" link that uses the tevent's id. 

                tevent.SentNotificationStarting = true;
                this.database.Update(tevent);
            }
        }

        public static string AbbreviateName(string firstname, string lastname)
        {
            string lastinitial = !string.IsNullOrEmpty(lastname) ? lastname.Substring(0, 1) : "";
            return $"{firstname} {lastinitial}.";
        }

    }
}

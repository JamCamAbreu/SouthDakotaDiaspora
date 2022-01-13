using Data.Models;
using Site.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Site.Models.Games
{
    public class GameInfo
    {
        public int ActivityId { get; set; }
        public string Platform { get; set; }
        public string PlatformAbbreviated { get; set; }
        public string Name { get; set; }
        public string NameAbbreviated { get; set; }
        public string Description { get; set; }
        public string DescriptionAbbreviated { get; set; }
        public string Url { get; set; }
        public string UrlAbbreviated { get; set; }
        public bool UrlWellFormatted { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ReleaseDate { get; set; }
        public GameInfo() { }
        public GameInfo(Game game)
        {
            this.ActivityId = game.ActivityId;
            this.Platform = game.Platform.ToString();
            this.PlatformAbbreviated = GlobalMethods.Abbreviate(game.Platform.ToString(), GlobalMethods.MAX_CHARACTERS_MEDIUM);
            this.Name = game.Name;
            this.NameAbbreviated = GlobalMethods.Abbreviate(game.Name, GlobalMethods.MAX_CHARACTERS_LARGE);
            this.Description = game.Description;
            this.DescriptionAbbreviated = GlobalMethods.Abbreviate(game.Description, GlobalMethods.MAX_CHARACTERS_LARGE);

            this.Url = game.WebsiteUrl;
            Uri siteuri;
            if (Uri.TryCreate(this.Url, UriKind.Absolute, out siteuri) && siteuri != null && !string.IsNullOrEmpty(siteuri.Host))
            {
                this.UrlAbbreviated = GlobalMethods.Abbreviate(siteuri.Host, GlobalMethods.MAX_CHARACTERS_LARGE);
                this.UrlWellFormatted = true;
            }
            else
            {
                this.UrlAbbreviated = GlobalMethods.Abbreviate(game.WebsiteUrl, GlobalMethods.MAX_CHARACTERS_LARGE);
                this.UrlWellFormatted = false;
            }
            this.ReleaseDate = game.ReleaseDate;
        }
    }
}
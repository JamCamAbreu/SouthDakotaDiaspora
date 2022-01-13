using Data.Models;
using Site.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models.Shows
{
    public class ShowInfo
    {
        public int ActivityId { get; set; }
        public int? NumberOfPages { get; set; }
        public string Platform { get; set; }
        public string PlatformAbbreviated { get; set; }
        public string Name { get; set; }
        public string NameAbbreviated { get; set; }
        public string Description { get; set; }
        public string DescriptionAbbreviated { get; set; }
        public string Url { get; set; }
        public string UrlAbbreviated { get; set; }
        public bool UrlWellFormatted { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public ShowInfo() { }
        public ShowInfo(Show show)
        {
            this.ActivityId = show.ActivityId;
            this.Platform = show.Platform.ToString();
            this.PlatformAbbreviated = GlobalMethods.Abbreviate(show.Platform.ToString(), GlobalMethods.MAX_CHARACTERS_MEDIUM);
            this.Name = show.Name;
            this.NameAbbreviated = GlobalMethods.Abbreviate(show.Name, GlobalMethods.MAX_CHARACTERS_LARGE);
            this.Description = show.Description;
            this.DescriptionAbbreviated = GlobalMethods.Abbreviate(show.Description, GlobalMethods.MAX_CHARACTERS_LARGE);

            this.Url = show.WebsiteUrl;
            Uri siteuri;
            if (Uri.TryCreate(this.Url, UriKind.Absolute, out siteuri) && siteuri != null && !string.IsNullOrEmpty(siteuri.Host))
            {
                this.UrlAbbreviated = GlobalMethods.Abbreviate(siteuri.Host, GlobalMethods.MAX_CHARACTERS_LARGE);
                this.UrlWellFormatted = true;
            }
            else
            {
                this.UrlAbbreviated = GlobalMethods.Abbreviate(show.WebsiteUrl, GlobalMethods.MAX_CHARACTERS_LARGE);
                this.UrlWellFormatted = false;
            }
            this.ReleaseDate = show.ReleaseDate;
        }
    }
}
using Data.Models;
using Site.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models.Projects
{
    public class ProjectInfo
    {
        public int ActivityId { get; set; }
        public DateTime? DateStarted { get; set; }
        public string Platform { get; set; }
        public string PlatformAbbreviation { get; set; }
        public string Name { get; set; }
        public string NameAbbreviated { get; set; }
        public string Description { get; set; }
        public string DescriptionAbbreviated { get; set; }
        public string Url { get; set; }
        public string UrlAbbreviated { get; set; }
        public bool UrlWellFormatted { get; set; }
        public string ProjectOwner { get; set; }
        public ProjectInfo() { }
        public ProjectInfo(Project project)
        {
            this.ActivityId = project.ActivityId;

            this.DateStarted = project.DateStarted;

            this.Platform = project.Platform.ToString();
            this.PlatformAbbreviation = GlobalMethods.Abbreviate(project.Platform.ToString(), GlobalMethods.MAX_CHARACTERS_MEDIUM);
            this.Name = project.Name;
            this.NameAbbreviated = GlobalMethods.Abbreviate(project.Name, GlobalMethods.MAX_CHARACTERS_LARGE);

            this.Description = project.Description;
            this.DescriptionAbbreviated= GlobalMethods.Abbreviate(project.Description, GlobalMethods.MAX_CHARACTERS_HUGE);

            this.Url = project.WebsiteUrl;
            Uri siteuri;
            if (Uri.TryCreate(this.Url, UriKind.Absolute, out siteuri) && siteuri != null && !string.IsNullOrEmpty(siteuri.Host))
            {
                this.UrlAbbreviated = GlobalMethods.Abbreviate(siteuri.Host, GlobalMethods.MAX_CHARACTERS_LARGE);
                this.UrlWellFormatted = true;
            }
            else
            {
                this.UrlAbbreviated = GlobalMethods.Abbreviate(project.WebsiteUrl, GlobalMethods.MAX_CHARACTERS_LARGE);
                this.UrlWellFormatted = false;
            }

            if (project.ProjectOwner != null)
            {
                this.ProjectOwner = GlobalMethods.AbbreviateName(project.ProjectOwner.FirstName, project.ProjectOwner.LastName);
            }
        }
    }
}
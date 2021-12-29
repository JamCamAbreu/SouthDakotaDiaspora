using Data.Models;
using Site.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models.Books
{
    public class BookInfo
    {
        public int ActivityId { get; set; }
        public DateTime PublishDate { get; set; }
        public int NumberOfPages { get; set; }
        public string Platform { get; set; }
        public string PlatformAbbreviation { get; set; }
        public string Name { get; set; }
        public string NameAbbreviated { get; set; }
        public string Url { get; set; }
        public string UrlAbbreviated { get; set; }
        public bool UrlWellFormatted { get; set; }
        public string Author { get; set; }
        public string AuthorAbbreviated { get; set; }
        public BookInfo() { }
        public BookInfo(Book book)
        {
            this.ActivityId = book.ActivityId;
            this.PublishDate = book.PublishDate;
            this.NumberOfPages = book.NumberPages;
            this.Platform = book.Platform.ToString();
            this.PlatformAbbreviation = GlobalMethods.Abbreviate(book.Platform.ToString(), GlobalMethods.MAX_CHARACTERS_MEDIUM);
            this.Name = book.Name;
            this.NameAbbreviated = GlobalMethods.Abbreviate(book.Name, GlobalMethods.MAX_CHARACTERS_LARGE);

            this.Url = book.WebsiteUrl;
            Uri siteuri;
            if (Uri.TryCreate(this.Url, UriKind.Absolute, out siteuri) && siteuri != null && !string.IsNullOrEmpty(siteuri.Host))
            {
                this.UrlAbbreviated = GlobalMethods.Abbreviate(siteuri.Host, GlobalMethods.MAX_CHARACTERS_LARGE);
                this.UrlWellFormatted = true;
            }
            else
            {
                this.UrlAbbreviated = GlobalMethods.Abbreviate(book.WebsiteUrl, GlobalMethods.MAX_CHARACTERS_LARGE);
                this.UrlWellFormatted = false;
            }
            
            this.Author = book.Author;
            this.AuthorAbbreviated = GlobalMethods.Abbreviate(book.Author, GlobalMethods.MAX_CHARACTERS_MEDIUM);
        }
    }
}
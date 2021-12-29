using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models.Books
{
    public class BooksIndexViewModel
    {
        public BookInfo Scaffolding { get; set; }
        public List<BookInfo> Books { get; set; }
        public BooksIndexViewModel()
        {
            this.Scaffolding = new BookInfo();
            this.Books = new List<BookInfo>();
        }
    }
}
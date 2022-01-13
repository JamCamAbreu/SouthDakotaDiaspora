using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models.Shows
{
    public class ShowsIndexViewModel
    {
        public ShowInfo Scaffolding { get; set; }
        public List<ShowInfo> Shows { get; set; }
        public ShowsIndexViewModel()
        {
            this.Scaffolding = new ShowInfo();
            this.Shows = new List<ShowInfo>();
        }
    }
}
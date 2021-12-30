using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models.Projects
{
    public class ProjectIndexViewModel
    {
        public ProjectInfo Scaffolding { get; set; }
        public List<ProjectInfo> Projects { get; set; }
        public ProjectIndexViewModel()
        {
            this.Scaffolding = new ProjectInfo();
            this.Projects = new List<ProjectInfo>();
        }
    }
}
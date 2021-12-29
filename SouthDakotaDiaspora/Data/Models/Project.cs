using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Project : Activity
    {
        [DataType(DataType.DateTime)]
        [Display(Name="Date Started")]
        public DateTime DateStarted { get; set; }
        [Display(Name = "Project Owner")]
        public User ProjectOwner { get; set; }
        [Display(Name = "Project Developers")]
        public List<User> ProjectDevelopers { get; set; }

        [NotMapped]
        public static string ACTIVITY_TYPE = "Project";
        public Project()
        {
            this.ActivityType = ACTIVITY_TYPE;
        }
    }
}

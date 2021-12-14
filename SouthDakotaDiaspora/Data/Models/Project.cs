using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}

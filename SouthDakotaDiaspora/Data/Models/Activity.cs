using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }
        [Required]
        public PlatformType Platform { get; set; }
        public string WebsiteUrl { get; set; }
        public List<Comment> Comments { get; set; }

        public ICollection<TimelineEvent> TimelineEvents { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
    }
}

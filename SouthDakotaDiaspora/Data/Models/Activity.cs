using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Activity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }
        [Required]
        public PlatformType Platform { get; set; }
        public List<Comment> Comments { get; set; }
    }
}

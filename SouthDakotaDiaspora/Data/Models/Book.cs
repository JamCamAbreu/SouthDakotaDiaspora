using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Book : Activity
    {
        [DataType(DataType.DateTime)]
        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
        [Display(Name = "Number of Pages")]
        public int NumberPages { get; set; }


        [NotMapped]
        public static string ACTIVITY_TYPE = "Book";
        public Book()
        {
            this.ActivityType = ACTIVITY_TYPE;
        }
    }
}

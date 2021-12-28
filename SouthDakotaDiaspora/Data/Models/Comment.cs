using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [MaxLength(256)]
        public string Message { get; set; }
        public User UserId { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}

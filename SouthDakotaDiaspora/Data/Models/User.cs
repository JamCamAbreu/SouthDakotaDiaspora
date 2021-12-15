using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(32)]
        public string FirstName { get; set; }
        [MaxLength(32)]
        public string LastName { get; set; }
        [MaxLength(64)]
        public string Username { get; set; }
        [MaxLength(32)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public List<Comment> Comments { get; set; }
    }
}

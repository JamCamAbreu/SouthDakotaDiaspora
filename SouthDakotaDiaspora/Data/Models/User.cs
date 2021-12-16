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
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [MaxLength(32)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [MaxLength(64)]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [MaxLength(32)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public List<Comment> Comments { get; set; }
        [Required]
        [Display(Name = "Role")]
        public UserRoleType UserRole { get; set; } = UserRoleType.Contributor;
    }
}

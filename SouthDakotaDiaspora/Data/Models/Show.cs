using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Show : Activity
    {
        [NotMapped]
        public static string ACTIVITY_TYPE = "Show";
        public Show()
        {
            this.ActivityType = ACTIVITY_TYPE;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Game : Activity
    {

        [NotMapped]
        public static string ACTIVITY_TYPE = "Game";
        public Game()
        {
            this.ActivityType = ACTIVITY_TYPE;
        }
    }
}

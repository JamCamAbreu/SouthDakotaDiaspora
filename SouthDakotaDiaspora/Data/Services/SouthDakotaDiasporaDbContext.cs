using Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class SouthDakotaDiasporaDbContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TimelineEvent> TimelineEvents { get; set; }
        public List<Game> Games
        {
            get
            {
                return Activities.Select(a => a as Game).ToList();
            }
        }        
    }
}

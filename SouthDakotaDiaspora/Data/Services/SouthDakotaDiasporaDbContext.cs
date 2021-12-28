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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TimelineEvent> TimelineEvents { get; set; }
        public List<Show> Shows
        {
            get
            {
                return Activities.OfType<Show>().ToList();
            }
        }
        public List<Game> Games
        {
            get
            {
                return Activities.OfType<Game>().ToList();
            }
        }
        public List<Book> Books
        {
            get
            {
                return Activities.OfType<Book>().ToList();
            }
        }
        public List<Project> Projects
        {
            get
            {
                return Activities.OfType<Project>().ToList();
            }
        }
    }
}

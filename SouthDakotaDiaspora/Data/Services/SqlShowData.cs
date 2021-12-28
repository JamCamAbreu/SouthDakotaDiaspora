using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class SqlShowData : IShowData
    {
        private readonly SouthDakotaDiasporaDbContext database;
        public SqlShowData(SouthDakotaDiasporaDbContext db)
        {
            database = db;
        }

        public void Add(Show show)
        {
            database.Activities.Add(show);
            database.SaveChanges();
        }

        public Show Get(int id)
        {
            return database.Shows.FirstOrDefault(g => g.ActivityId == id);
        }

        public IEnumerable<Show> GetAll()
        {
            return database.Shows.OrderBy(g => g.Name);
        }

        public void Update(Show show)
        {
            Show existing = this.Get(show.ActivityId);
            if (existing != null)
            {
                existing.Name = show.Name;
                existing.Description = show.Description;
                existing.ReleaseDate = show.ReleaseDate;
                existing.WebsiteUrl = show.WebsiteUrl;
                existing.Platform = show.Platform;
                database.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Show show = this.Get(id);
            if (show != null)
            {
                database.Activities.Remove(show);
                database.SaveChanges();
            }
        }
    }
}

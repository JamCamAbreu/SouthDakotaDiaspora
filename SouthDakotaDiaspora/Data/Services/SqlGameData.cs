using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class SqlGameData : IGameData
    {
        private readonly SouthDakotaDiasporaDbContext database;
        public SqlGameData(SouthDakotaDiasporaDbContext db)
        {
            database = db;
        }

        public void Add(Game game)
        {
            database.Activities.Add(game);
            database.SaveChanges();
        }

        public Game Get(int id)
        {
            return database.Games.FirstOrDefault(g => g.ActivityId == id);
        }

        public IEnumerable<Game> GetAll()
        {
            return database.Games.OrderBy(g => g.Name);
        }

        public void Update(Game game)
        {
            Game existing = database.Games.Where(g => g.ActivityId == game.ActivityId).FirstOrDefault();
            if (existing != null)
            {
                existing.Name = game.Name;
                existing.Description = game.Description;
                existing.WebsiteUrl = game.WebsiteUrl;
                existing.ReleaseDate = game.ReleaseDate;
                existing.Platform = game.Platform;
                database.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Game game = database.Games.Where(g => g.ActivityId == id).FirstOrDefault();
            if (game != null)
            {
                database.Activities.Remove(game);
                database.SaveChanges();
            }
        }
    }
}

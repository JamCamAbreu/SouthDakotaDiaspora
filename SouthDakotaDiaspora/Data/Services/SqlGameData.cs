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
            database.Games.Add(game);
            database.SaveChanges();
        }

        public Game Get(int id)
        {
            return database.Games.FirstOrDefault(g => g.Id == id);
        }

        public IEnumerable<Game> GetAll()
        {
            return from g in database.Games
                   orderby g.Name
                   select g;
        }

        public void Update(Game game)
        {
            Game existing = database.Games.Find(game.Id);
            if (existing != null)
            {
                existing.Name = game.Name;
                existing.Description = game.Description;
                existing.ReleaseDate = game.ReleaseDate;
                existing.Platform = game.Platform;
                database.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var game = database.Games.Find(id);
            database.Games.Remove(game);
            database.SaveChanges();
        }
    }
}

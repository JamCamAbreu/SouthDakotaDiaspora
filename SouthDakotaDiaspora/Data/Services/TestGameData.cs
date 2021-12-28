using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class TestGameData : IGameData
    {
        List<Game> games { get; set; }
        public TestGameData()
        {
            this.games = new List<Game>()
            {
                new Game() { ActivityId = 1, Name = "Valheim", Platform = PlatformType.Steam, ReleaseDate = DateTime.Parse("12/14/2021 10:32 -0700")},
                new Game() { ActivityId = 2, Name = "Mario Kart 8", Platform = PlatformType.NintendoSwitch, ReleaseDate = DateTime.Parse("10/16/2020")},
                new Game() { ActivityId = 3, Name = "Troyes", Platform = PlatformType.BrowserSite, ReleaseDate = DateTime.Parse("01/01/2016")},
            };
        }
        public IEnumerable<Game> GetAll()
        {
            return this.games;
        }

        public Game Get(int id)
        {
            return games.FirstOrDefault(g => g.ActivityId == id);
        }

        public void Add(Game game)
        {
            game.ActivityId = this.games.Max(g => g.ActivityId) + 1;
            this.games.Add(game);
        }

        public void Delete(int id)
        {
            var game = this.Get(id);
            if (game != null)
            {
                this.games.Remove(game);
            }
        }
        public void Update(Game game)
        {
            Game existing = this.Get(game.ActivityId);
            if (existing != null)
            {
                existing.Name = game.Name;
                existing.Description = game.Description;
                existing.Platform = game.Platform;
                existing.ReleaseDate = game.ReleaseDate;
            }
        }
    }
}

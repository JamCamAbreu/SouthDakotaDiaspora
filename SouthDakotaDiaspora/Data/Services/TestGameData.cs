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
                new Game() { Id = 1, Name = "Valheim", Platform = PlatformType.Steam, Type = GameType.VideoGame, ReleaseDate = DateTime.Parse("02/02/2021")},
                new Game() { Id = 2, Name = "Mario Kart 8", Platform = PlatformType.NintendoSwitch, Type = GameType.VideoGame, ReleaseDate = DateTime.Parse("10/16/2020")},
                new Game() { Id = 3, Name = "Troyes", Platform = PlatformType.Browser, Type = GameType.BoardGame, ReleaseDate = DateTime.Parse("01/01/2016")},
            };
        }
        public IEnumerable<Game> GetAll()
        {
            return this.games;
        }
    }
}

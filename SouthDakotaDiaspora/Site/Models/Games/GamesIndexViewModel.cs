using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models.Games
{
    public class GamesIndexViewModel
    {
        public GameInfo Scaffolding { get; set; }
        public List<GameInfo> Books { get; set; }
        public GamesIndexViewModel()
        {
            this.Scaffolding = new GameInfo();
            this.Books = new List<GameInfo>();
        }
    }
}
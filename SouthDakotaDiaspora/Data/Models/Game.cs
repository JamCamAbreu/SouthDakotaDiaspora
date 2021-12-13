﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GameType Type { get; set; }
        public PlatformType Platform { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}

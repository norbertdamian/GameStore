using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models
{
    public class PublishedGame
    {
        public int PublisherID { get; set; }
        public int GameID { get; set; }
        public Publisher Publisher { get; set; }
        public Game Game { get; set; }
    }
}

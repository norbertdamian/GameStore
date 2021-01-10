using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.StoreViewModels
{
    public class PublishedGameData
    {
        public int GameID { get; set; }
        public string Title { get; set; }
        public bool IsPublished { get; set; }
    }
}

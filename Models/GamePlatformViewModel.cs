using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace GameStore.Models
{
    public class GamePlatformViewModel
    {
        public List<Game> Games { get; set; }
        public SelectList Platform { get; set; }
        public string GamePlatform { get; set; }
        public string SearchString { get; set; }
    }
}

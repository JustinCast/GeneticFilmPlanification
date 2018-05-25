using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Scene
    {
        // se calcula en base a las páginas que la escena ocupa en el guión
        public int Duration { get; set; }
        public int Pages { get; set; }
        public List<Actor> Actors = new List<Actor>(); 
        public Location Location { get; set; }
        public bool Schedule { get; set; } // true = dia, false = noche
        public bool assigned = false;
    }
}

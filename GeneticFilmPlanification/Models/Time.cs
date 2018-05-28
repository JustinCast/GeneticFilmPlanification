using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Time  
    {
        public int MaximunScriptPages { get; set; } // duración máxima establecida
        public List<Scene> Scenes = new List<Scene>();
        public List<Location> AvailableLocations = new List<Location>();
        public String TimeName { get; set; }

        public bool IfSceneIsAllowed(Scene s)
        {
            return (TotalPages() + s.Pages) < MaximunScriptPages;
        }

        public int TotalPages()
        {
            int cost = 0;
            Scenes.ForEach(s => cost += s.Pages);
            return cost;
        }
    }
}

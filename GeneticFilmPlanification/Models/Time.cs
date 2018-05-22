using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Time  
    {
        public int MaximunScriptPages { get; set; }
        public List<Scene> Scenes { get; set; }
        public List<Location> AvailableLocations { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Time  
    {
        int MaximunScriptPages { get; set; }
        List<Scene> Scenes { get; set; }
        List<Location> AvailableLocations { get; set; }
    }
}

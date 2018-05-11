using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Movie
    {
        List<Scene> MovieScenes { get; set; }
        List<Actor> AllActors { get; set; }
        List<Location> MovieLocations { get; set; }
        List<WorkingDay> AvailableWorkingDays { get; set; }
    }
}

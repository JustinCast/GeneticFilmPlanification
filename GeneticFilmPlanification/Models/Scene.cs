using GeneticFilmPlanification.Models.AuxiliarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Scene
    {
        int Duration { get; set; }
        List<Actor> Actors { get; set; }
        List<Location> SceneLocations { get; set; }
        Schedule SceneSchedule { get; set; }
    }
}

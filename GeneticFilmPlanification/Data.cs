using GeneticFilmPlanification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification
{
    class Data
    {
        static List<FilmingCalendar> Scenarios = new List<FilmingCalendar>();
        static List<Actor> Actors = new List<Actor>();
        static List<Location> Locations = new List<Location>();

        public static void createActors(int numberOfActors)
        {
            for (int i = 0; i <= numberOfActors; i++)
            {
                Actor newActor = new Actor();
                newActor.CostPerDay = 0;
                newActor.ID = i.ToString();
                Actors.Add(newActor);
            }
        }

        public static void createLocation(int numberOfLocations)
        {
            for (int i = 0; i <= numberOfLocations; i++)
            {
                Location newLocation = new Location();
                newLocation.ID = i.ToString();
                newLocation.InUse = false;
                Locations.Add(newLocation);
            }
        }
        public static void createScenes(int numberOfScenes,FilmingCalendar calendar) {
            int pages = 5;
            for (int i = 0; i <= numberOfScenes; i++) {
                Scene newScene = new Scene();
                newScene.Pages = pages+1;
                newScene.Duration = pages * 5;// cada pagina tiene un valor de 5 minutos
                
                calendar.Scenes.Add(newScene);
            }
        }

        public static void createScenario(int numberOfLocation, int numberOfActors, int numberOfScenes, FilmingCalendar calendar) {
            createLocation(numberOfLocation);
            createActors(numberOfActors);
            createScenes(numberOfScenes,calendar);
        }
    }
}

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
            for (int i = 1; i <= numberOfActors; i++)
            {
                Actor newActor = new Actor();
                newActor.CostPerDay = 0;
                newActor.ID = i.ToString();
                Actors.Add(newActor);
            }
        }

        public static void createLocation(int numberOfLocations)
        {
            for (int i = 1; i <= numberOfLocations; i++)
            {
                Location newLocation = new Location();
                newLocation.ID = i.ToString();
                newLocation.InUse = false;
                Locations.Add(newLocation);
            }
        }

        public static void createCalendar() {
            for (int i=1;i<=4;i++) {
                FilmingCalendar newCalendar = new FilmingCalendar();
                Scenarios.Add(newCalendar);
            }
        }

        public static void createScenes(int numberOfScenes,int calendar, int actorsPerScene) {
            int pages = 5;
            Boolean flag = true;
            for (int i = 0; i <= numberOfScenes; i++) {
                Scene newScene = new Scene();
                newScene.Pages = pages+1;
                newScene.Duration = pages * 5;// cada pagina tiene un valor de 5 minutos
                pages++;
                for (int j=1;j<=actorsPerScene;j++) {
                    newScene.ActorsID.Add(Actors[j].ID.ToString());
                }
                newScene.Location = Locations[actorsPerScene];
                Scenarios[calendar].Scenes.Add(newScene);
                if (flag==true) {
                    newScene.Schedule = flag;
                    flag = false;
                }
                else {
                    newScene.Schedule = flag;
                    flag = true;
                }
            }
        }

        public static void createScenario(int numberOfLocation, int numberOfActors, int numberOfScenes, int calendar,
        int actorsPerScene)
        {
            createCalendar();
            createLocation(numberOfLocation);
            createActors(numberOfActors);
            createScenes(numberOfScenes,calendar,actorsPerScene);
        }

        public static Actor searchActor(string idActor) {
            foreach (Actor actor in Actors) {
                if (actor.ID.Equals(idActor)) {
                    return actor;
                }
            }
            return null;
        }

        public static void printScenarios() {
            Console.WriteLine("Numero de scenarios "+ Scenarios.Count);
            for (int i=1; i<Scenarios.Count;i++) {
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine("Scenario number "+i);
                Console.WriteLine("Number of scenes " + Scenarios[i].Scenes.Count);
                foreach (Scene scene in Scenarios[i].Scenes) {
                    Console.WriteLine("Number scene: ");
                    Console.WriteLine("Duration: "+scene.Duration);
                    Console.WriteLine("Pages: "+scene.Pages);
                    Console.WriteLine("Schedule: "+scene.Schedule);
                    Console.WriteLine("Location: "+scene.Location);
                    Console.WriteLine("*************************Actors*************************");
                    foreach (String idActor in scene.ActorsID) {
                        Actor currentActor = searchActor(idActor);
                        Console.WriteLine("Id actor: "+currentActor.ID);
                        Console.WriteLine("Coste per day: "+currentActor.CostPerDay);
                        Console.WriteLine("First participation: "+ currentActor.FirstParticipation);
                        Console.WriteLine("Last participation: "+currentActor.LastParticipation);
                    }
                }
            }
        }
    }
}

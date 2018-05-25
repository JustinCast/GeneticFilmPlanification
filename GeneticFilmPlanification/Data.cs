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
                newActor.CostPerDay = i+2;
                newActor.ID = i.ToString();
                Actors.Add(newActor);
            }
        }

        public static void createLocations(int numberOfLocations)
        {
            for (int i = 1; i <= numberOfLocations; i++)
            {
                Location newLocation = new Location();
                newLocation.ID = i.ToString();
                newLocation.InUse = false;
                Locations.Add(newLocation);
            }
        }

        public static FilmingCalendar searchCalendar(int position) {
            return Scenarios[position];
        }

        public static void createCalendar() {
            FilmingCalendar newCalendar = new FilmingCalendar();
            Scenarios.Add(newCalendar);
        }

        public static Location searchLocation(string idLocation)
        {
            foreach (Location location in Locations)
            {
                if (location.ID.Equals(idLocation))
                {
                    return location;
                }
            }
            return null;
        }

        public static void createScene(int pages, bool shedule,string idLocation, int calendar ) {
            Scene newScene = new Scene();
            Location currentLocation = searchLocation(idLocation);
            newScene.Location = currentLocation;
            currentLocation.InUse = true;
            newScene.Pages = pages;
            newScene.Schedule = shedule;
            FilmingCalendar currentCalendar = searchCalendar(calendar);
            currentCalendar.Scenes.Add(newScene);
        }

        public static void assignActorToScene(string idActor,int positionScene, int positionCalendar) {
            Actor actor = searchActor(idActor);
            FilmingCalendar calendar = searchCalendar(positionCalendar);
            calendar.Scenes[positionScene].Actors.Add(actor);
        }

        public static void createScenario1(int numberOfLocation, int numberOfActors, int calendar)
        {
            createCalendar();
            createLocations(numberOfLocation);
            createActors(numberOfActors);
            // Contendrá 10 escenas, 5 localidades,40 actores disponibles, 
            //las relaciones de las escenas en este escenario serán:
            //Poseerán 4 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(10, true,"1", calendar);//pages, shedule, idLocation, posicion de calendar
            assignActorToScene("1",0,0);// idactor, positionScene, positionCalendar
            assignActorToScene("2", 0, 0);
            assignActorToScene("4", 0, 0);
            assignActorToScene("5", 0, 0);

            createScene(12, true, "2", calendar);
            assignActorToScene("6", 1, 0);
            assignActorToScene("7", 1, 0);
            assignActorToScene("8", 1, 0);
            assignActorToScene("9", 1, 0);

            createScene(15, true, "3", calendar);
            assignActorToScene("17", 2, 0);
            assignActorToScene("16", 2, 0);
            assignActorToScene("15", 2, 0);
            assignActorToScene("14", 2, 0);

            createScene(9, true, "4", calendar);
            assignActorToScene("40", 3, 0);
            assignActorToScene("39", 3, 0);
            assignActorToScene("38", 3, 0);
            assignActorToScene("37", 3, 0);

            createScene(8, true, "5", calendar);
            assignActorToScene("40", 4, 0);
            assignActorToScene("39", 4, 0);
            assignActorToScene("38", 4, 0);
            assignActorToScene("37", 4, 0);

            createScene(12, false, "2", calendar);
            assignActorToScene("6", 5, 0);
            assignActorToScene("17", 5, 0);
            assignActorToScene("16", 5, 0);
            assignActorToScene("15", 5, 0);

            createScene(20, false, "4", calendar);
            assignActorToScene("6", 6, 0);
            assignActorToScene("7", 6, 0);
            assignActorToScene("8", 6, 0);
            assignActorToScene("10", 6, 0);

            createScene(9, false, "1", calendar);
            assignActorToScene("12", 7, 0);
            assignActorToScene("15", 7, 0);
            assignActorToScene("16", 7, 0);
            assignActorToScene("17", 7, 0);

            createScene(15, false, "5", calendar);
            assignActorToScene("12", 8, 0);
            assignActorToScene("39", 8, 0);
            assignActorToScene("38", 8, 0);
            assignActorToScene("37", 8, 0);

            createScene(21, false, "3", calendar);
            assignActorToScene("1", 9, 0);
            assignActorToScene("2", 9, 0);
            assignActorToScene("4", 9, 0);
            assignActorToScene("5", 9, 0);
            Actors.Clear();
            Locations.Clear();
        }

        public static void createScenario2(int numberOfLocation, int numberOfActors, int calendar)
        {
            createCalendar();
            createLocations(numberOfLocation);
            createActors(numberOfActors);

            //Escenario dos: Contendrá 15 escenas, 6 localidades, 60 actores disponibles, las relaciones de las escenas en este escenario serán:
            //Poseerán 3 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(23, true, "1", calendar);//pages, shedule, idLocation, posicion de calendar
            assignActorToScene("1", 0, 1);// idactor, positionScene, positionCalendar
            assignActorToScene("4", 0, 1);
            assignActorToScene("20", 0, 1);

            createScene(21, true, "6", calendar);
            assignActorToScene("1", 1, 1);
            assignActorToScene("20", 1, 1);
            assignActorToScene("19", 1, 1);

            createScene(10, true, "5", calendar);
            assignActorToScene("44", 2, 1);
            assignActorToScene("55", 2, 1);
            assignActorToScene("56", 2, 1);

            createScene(10, true, "3", calendar);
            assignActorToScene("1", 3, 1);
            assignActorToScene("21", 3, 1);
            assignActorToScene("4", 3, 1);

            createScene(11, true, "4", calendar);
            assignActorToScene("55", 4, 1);
            assignActorToScene("19", 4, 1);
            assignActorToScene("44", 4, 1);

            createScene(12, true, "2", calendar);
            assignActorToScene("19", 5, 1);
            assignActorToScene("4", 5, 1);
            assignActorToScene("20", 5, 1);

            createScene(13, true, "1", calendar);
            assignActorToScene("40", 6, 1);
            assignActorToScene("44", 6, 1);
            assignActorToScene("1", 6, 1);

            createScene(16, true, "2", calendar);
            assignActorToScene("9", 7, 1);
            assignActorToScene("12", 7, 1);
            assignActorToScene("10", 7, 1);

            createScene(14, false, "3", calendar);
            assignActorToScene("1", 8, 1);
            assignActorToScene("44", 8, 1);
            assignActorToScene("10", 8, 1);

            createScene(15, false, "4", calendar);
            assignActorToScene("1", 9, 1);
            assignActorToScene("60", 9, 1);
            assignActorToScene("4", 9, 1);

            createScene(22, false, "6", calendar);
            assignActorToScene("40", 10, 1);
            assignActorToScene("33", 10, 1);
            assignActorToScene("10", 10, 1);

            createScene(23, false, "6", calendar);
            assignActorToScene("19", 11, 1);
            assignActorToScene("1", 11, 1);
            assignActorToScene("30", 11, 1);

            createScene(24, false, "5", calendar);
            assignActorToScene("33", 12, 1);
            assignActorToScene("60", 12, 1);
            assignActorToScene("40", 12, 1);

            createScene(11, false, "4", calendar);
            assignActorToScene("60", 13, 1);
            assignActorToScene("30", 13, 1);
            assignActorToScene("4", 13, 1);

            createScene(19, false, "3", calendar);
            assignActorToScene("21", 14, 1);
            assignActorToScene("20", 14, 1);
            assignActorToScene("4", 14, 1);

            Actors.Clear();
            Locations.Clear();
        }

        public static void createScenario4(int numberOfLocation, int numberOfActors, int calendar)
        {
            createCalendar();
            createLocations(numberOfLocation);
            createActors(numberOfActors);
            //Escenario cuatro: Contendrá 20 escenas, 8 localidades, 80 actores disponibles, las relaciones de las escenas en este escenario serán:
            //Poseerán 4 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(24, true, "1", calendar);//pages, shedule, idLocation, posicion de calendar
            assignActorToScene("80", 0, 3);// idactor, positionScene, positionCalendar
            assignActorToScene("70", 0, 3);
            assignActorToScene("60", 0, 3);
            assignActorToScene("61", 0, 3);

            createScene(24, true, "8", calendar);
            assignActorToScene("10", 1, 3);
            assignActorToScene("4", 1, 3);
            assignActorToScene("11", 1, 3);
            assignActorToScene("80", 1, 3);

            createScene(14, true, "7", calendar);
            assignActorToScene("60", 2, 3);
            assignActorToScene("3", 2, 3);
            assignActorToScene("61", 2, 3);
            assignActorToScene("6", 2, 3);

            createScene(16, true, "1", calendar);
            assignActorToScene("80", 3, 3);
            assignActorToScene("77", 3, 3);
            assignActorToScene("60", 3, 3);
            assignActorToScene("19", 3, 3);

            createScene(19, true, "1", calendar);
            assignActorToScene("20", 4, 3);
            assignActorToScene("55", 4, 3);
            assignActorToScene("4", 4, 3);
            assignActorToScene("10", 4, 3);

            createScene(18, true, "1", calendar);
            assignActorToScene("61", 5, 3);
            assignActorToScene("77", 5, 3);
            assignActorToScene("55", 5, 3);
            assignActorToScene("11", 5, 3);

            createScene(23, true, "8", calendar);
            assignActorToScene("77", 6, 3);
            assignActorToScene("16", 6, 3);
            assignActorToScene("60", 6, 3);
            assignActorToScene("33", 6, 3);

            createScene(11, true, "8", calendar);
            assignActorToScene("22", 7, 3);
            assignActorToScene("24", 7, 3);
            assignActorToScene("77", 7, 3);
            assignActorToScene("74", 7, 3);

            createScene(14, true, "6", calendar);
            assignActorToScene("10", 8, 3);
            assignActorToScene("30", 8, 3);
            assignActorToScene("32", 8, 3);
            assignActorToScene("55", 8, 3);

            createScene(15, true, "4", calendar);
            assignActorToScene("4", 9, 3);
            assignActorToScene("19", 9, 3);
            assignActorToScene("16", 9, 3);
            assignActorToScene("77", 9, 3);

            createScene(16, false, "3", calendar);
            assignActorToScene("10", 10, 3);
            assignActorToScene("12", 10, 3);
            assignActorToScene("11", 10, 3);
            assignActorToScene("3", 10, 3);

            createScene(20, false, "1", calendar);
            assignActorToScene("14", 11, 3);
            assignActorToScene("13", 11, 3);
            assignActorToScene("79", 11, 3);
            assignActorToScene("15", 11, 3);

            createScene(19, false, "3", calendar);
            assignActorToScene("13", 12, 3);
            assignActorToScene("14", 12, 3);
            assignActorToScene("79", 12, 3);
            assignActorToScene("44", 12, 3);

            createScene(25, false, "5", calendar);
            assignActorToScene("14", 13, 3);
            assignActorToScene("78", 13, 3);
            assignActorToScene("15", 13, 3);
            assignActorToScene("79", 13, 3);

            createScene(22, false, "6", calendar);
            assignActorToScene("8", 14, 3);
            assignActorToScene("7", 14, 3);
            assignActorToScene("9", 14, 3);
            assignActorToScene("55", 14, 3);

            createScene(14, false, "6", calendar);
            assignActorToScene("3", 15, 3);
            assignActorToScene("79", 15, 3);
            assignActorToScene("15", 15, 3);
            assignActorToScene("14", 15, 3);

            createScene(19, false, "7", calendar);
            assignActorToScene("15", 16, 3);
            assignActorToScene("33", 16, 3);
            assignActorToScene("2", 16, 3);
            assignActorToScene("1", 16, 3);

            createScene(18, false, "2", calendar);
            assignActorToScene("33", 17, 3);
            assignActorToScene("55", 17, 3);
            assignActorToScene("78", 17, 3);
            assignActorToScene("22", 17, 3);

            createScene(17, false, "8", calendar);
            assignActorToScene("79", 18, 3);
            assignActorToScene("19", 18, 3);
            assignActorToScene("77", 18, 3);
            assignActorToScene("12", 18, 3);

            createScene(18, false, "3", calendar);
            assignActorToScene("78", 19, 3);
            assignActorToScene("34", 19, 3);
            assignActorToScene("64", 19, 3);
            assignActorToScene("12", 19, 3);

            Actors.Clear();
            Locations.Clear();
        }

        public static void createScenario3(int numberOfLocation, int numberOfActors, int calendar)
        {
            createCalendar();
            createLocations(numberOfLocation);
            createActors(numberOfActors);

            //Escenario tres: Contendrá 17 escenas, 7 localidades, 100 actores disponibles, las relaciones de las escenas en este escenario serán:
            //Poseerán 5 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(13, true, "1", calendar);//pages, shedule, idLocation, posicion de calendar
            assignActorToScene("1", 0, 2);// idactor, positionScene, positionCalendar
            assignActorToScene("100", 0, 2);
            assignActorToScene("80", 0, 2);
            assignActorToScene("8", 0, 2);
            assignActorToScene("7", 0, 2);

            createScene(20, true, "2", calendar);
            assignActorToScene("5", 1, 2);
            assignActorToScene("1", 1, 2);
            assignActorToScene("67", 1, 2);
            assignActorToScene("34", 1, 2);
            assignActorToScene("30", 1, 2);

            createScene(24, true, "7", calendar);
            assignActorToScene("20", 2, 2);
            assignActorToScene("8", 2, 2);
            assignActorToScene("99", 2, 2);
            assignActorToScene("1", 2, 2);
            assignActorToScene("30", 2, 2);

            createScene(11, true, "7", calendar);
            assignActorToScene("100", 3, 2);
            assignActorToScene("22", 3, 2);
            assignActorToScene("7", 3, 2);
            assignActorToScene("35", 3, 2);
            assignActorToScene("94", 3, 2);

            createScene(18, true, "6", calendar);
            assignActorToScene("14", 4, 2);
            assignActorToScene("40", 4, 2);
            assignActorToScene("20", 4, 2);
            assignActorToScene("69", 4, 2);
            assignActorToScene("68", 4, 2);

            createScene(13, true, "7", calendar);
            assignActorToScene("78", 5, 2);
            assignActorToScene("58", 5, 2);
            assignActorToScene("29", 5, 2);
            assignActorToScene("2", 5, 2);
            assignActorToScene("100", 5, 2);

            createScene(19, true, "4", calendar);
            assignActorToScene("78", 6, 2);
            assignActorToScene("28", 6, 2);
            assignActorToScene("20", 6, 2);
            assignActorToScene("30", 6, 2);
            assignActorToScene("40", 6, 2);

            createScene(17, true, "3", calendar);
            assignActorToScene("20", 7, 2);
            assignActorToScene("8", 7, 2);
            assignActorToScene("99", 7, 2);
            assignActorToScene("78", 7, 2);
            assignActorToScene("29", 7, 2);

            createScene(16, true, "3", calendar);
            assignActorToScene("58", 8, 2);
            assignActorToScene("1", 8, 2);
            assignActorToScene("8", 8, 2);
            assignActorToScene("22", 8, 2);
            assignActorToScene("78", 8, 2);

            createScene(21, false, "7", calendar);
            assignActorToScene("10", 9, 2);
            assignActorToScene("14", 9, 2);
            assignActorToScene("4", 9, 2);
            assignActorToScene("58", 9, 2);
            assignActorToScene("78", 9, 2);

            createScene(17, false, "4", calendar);
            assignActorToScene("98", 10, 2);
            assignActorToScene("65", 10, 2);
            assignActorToScene("1", 10, 2);
            assignActorToScene("78", 10, 2);
            assignActorToScene("28", 10, 2);

            createScene(24, false, "7", calendar);
            assignActorToScene("13", 11, 2);
            assignActorToScene("79", 11, 2);
            assignActorToScene("35", 11, 2);
            assignActorToScene("18", 11, 2);
            assignActorToScene("10", 11, 2);

            createScene(22, false, "4", calendar);
            assignActorToScene("99", 12, 2);
            assignActorToScene("3", 12, 2);
            assignActorToScene("30", 12, 2);
            assignActorToScene("100", 12, 2);
            assignActorToScene("4", 12, 2);

            createScene(23, false, "5", calendar);
            assignActorToScene("98", 13, 2);
            assignActorToScene("65", 13, 2);
            assignActorToScene("24", 13, 2);
            assignActorToScene("10", 13, 2);
            assignActorToScene("14", 13, 2);

            createScene(18, false, "6", calendar);
            assignActorToScene("17", 14, 2);
            assignActorToScene("3", 14, 2);
            assignActorToScene("2", 14, 2);
            assignActorToScene("10", 14, 2);
            assignActorToScene("11", 14, 2);

            createScene(17, false, "2", calendar);
            assignActorToScene("7", 15, 2);
            assignActorToScene("8", 15, 2);
            assignActorToScene("3", 15, 2);
            assignActorToScene("4", 15, 2);
            assignActorToScene("14", 15, 2);

            createScene(15, false, "1", calendar);
            assignActorToScene("67", 16, 2);
            assignActorToScene("68", 16, 2);
            assignActorToScene("66", 16, 2);
            assignActorToScene("2", 16, 2);
            assignActorToScene("1", 16, 2);
            Actors.Clear();
            Locations.Clear();
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
            Console.WriteLine("Numero de scenarios "+ Scenarios.Count+"\n");
            for (int i=1; i<=Scenarios.Count;i++) {
                Console.WriteLine("      Scenario number "+i + "      Number of scenes " + Scenarios[i - 1].Scenes.Count);
                foreach (Scene scene in Scenarios[i-1].Scenes) {
                    Console.WriteLine("--------------------------------------------------------------------------------------");
                    Console.WriteLine("                                       Escene                                        ");
                    Console.WriteLine("--------------------------------------------------------------------------------------");
                    Console.WriteLine("  Duration: " + scene.Duration + "   Pages: " + scene.Pages + 
                    "    Schedule: " + scene.Schedule + "   Location: " + scene.Location.ID );
                    Console.WriteLine("---------------------------------------Actors-----------------------------------------");
                    foreach (Actor actor in scene.Actors) {
                        Console.WriteLine("Id actor: "+actor.ID + "    Coste per day: " + actor.CostPerDay + "    First participation: " + actor.FirstParticipation +
                        "    Last participation: " + actor.LastParticipation + "\n");
                    }
                }
                Console.WriteLine("\n\n\n");
            }
        }
    }
}

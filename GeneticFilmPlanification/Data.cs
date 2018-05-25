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
        public static void createDays(Movie movie)
        {// crea los dias ademas de agregar los objetos de jornada dia y jornada noche con un maximo de paginas de 35
            foreach(Scenario scenario in movie.Scenarios) {
                for (int i = 1; i <= 40; i++)
                {
                    Day newDay = new Day();
                    newDay.DayNumber = i;
                    Time time1 = new Time();
                    time1.MaximunScriptPages = 35;
                    Time time2 = new Time();
                    time2.MaximunScriptPages = 35;
                    newDay.DayTime = time1;
                    newDay.NightTime = time2;
                    scenario.Days.Add(newDay);
                }
            }  
        }

        public static void printDay(Movie movie) {
            int numberScenario = 1;
            for (int i=0; i< movie.Scenarios.Count;i++) {
                foreach (Day day in movie.Scenarios[i].Days)    
                {
                    Console.WriteLine("Scenario number " + numberScenario);
                    numberScenario += 1;
                    Console.WriteLine("-----------------------------------Day number "+day.DayNumber+" --------------------------");
                    Console.WriteLine("Day time...");
                    Console.WriteLine("Avalable Locations...");
                    foreach (Location time in day.DayTime.AvailableLocations) {

                    }
                    Console.WriteLine(" Scenes...");
                    foreach (Scene time in day.DayTime.Scenes)
                    {

                    }
                    Console.WriteLine("Nigth time...");
                    Console.WriteLine("Avalable Locations...");
                    foreach (Location time in day.DayTime.AvailableLocations)
                    {

                    }
                    Console.WriteLine(" Scenes...");
                    foreach (Scene time in day.DayTime.Scenes)
                    {

                    }
                }
            }
            


        }

        public static void createScenariosOfMovie(Movie movie) {
            for (int i=0; i<4;i++) {
                Scenario newScenario = new Scenario();
                movie.Scenarios.Add(newScenario);
            }
        }

        public static void createActors(int numberOfActors,Movie movie, int positionScenario)
        {
            for (int i = 1; i <= numberOfActors; i++)
            {
                Actor newActor = new Actor();
                newActor.CostPerDay = i+2;
                newActor.ID = i.ToString();
                movie.Scenarios[positionScenario].Actors.Add(newActor);
            }
        }

        public static void createLocations(int numberOfLocations, Movie movie, int positionScenario)
        {
            for (int i = 1; i <= numberOfLocations; i++)
            {
                Location newLocation = new Location();
                newLocation.ID = i.ToString();
                newLocation.InUse = false;
                movie.Scenarios[positionScenario].Locations.Add(newLocation);
            }
        }


        public static void createCalendar(Movie movie, int positionScenario) {
            FilmingCalendar newCalendar = new FilmingCalendar();
            movie.Scenarios[positionScenario].FilmingCalendars.Add(newCalendar);
        }

        public static Location searchLocation(string idLocation, Movie movie, int positionScenario)
        {
            foreach (Location location in movie.Scenarios[positionScenario].Locations)
            {
                if (location.ID.Equals(idLocation))
                {
                    return location;
                }
            }
            return null;
        }

        public static void createScene(int pages, bool shedule,string idLocation, Movie movie, int positionScenario  ) {
            Scene newScene = new Scene();
            Location currentLocation = searchLocation(idLocation,movie,positionScenario);
            newScene.Location = currentLocation;
            currentLocation.InUse = true;
            newScene.Pages = pages;
            newScene.Schedule = shedule;
            movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes.Add(newScene);
        }

        public static void assignActorToScene(string idActor,int positionScene, Movie movie, int positionScenario) {
            Actor actor = searchActor(idActor, movie, positionScenario);
            movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes[positionScene].Actors.Add(actor);
        }

        public static Actor searchActor(string idActor, Movie movie, int positionScenario)
        {
            foreach (Actor actor in movie.Scenarios[positionScenario].Actors)
            {
                if (actor.ID.Equals(idActor))
                {
                    return actor;
                }
            }
            return null;
        }

        public static void createScenario1(int numberOfLocation, int numberOfActors, Movie movie, int positionScenario)
        {
            createCalendar(movie, positionScenario);
            createLocations(numberOfLocation, movie,positionScenario);
            createActors(numberOfActors, movie, positionScenario);
            // Contendrá 10 escenas, 5 localidades,40 actores disponibles, 
            //las relaciones de las escenas en este escenario serán:
            //Poseerán 4 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(10, true,"1", movie, positionScenario);//pages, shedule, idLocation, pelicula, posicion del escenario
            assignActorToScene("1",0, movie, positionScenario);// idactor, positionScene, pelicula, posicion del escenario
            assignActorToScene("2", 0, movie, positionScenario);
            assignActorToScene("4", 0, movie, positionScenario);
            assignActorToScene("5", 0, movie, positionScenario);

            createScene(12, true, "2", movie, positionScenario);
            assignActorToScene("6", 1, movie, positionScenario);
            assignActorToScene("7", 1, movie, positionScenario);
            assignActorToScene("8", 1, movie, positionScenario);
            assignActorToScene("9", 1, movie, positionScenario);

            createScene(15, true, "3", movie, positionScenario);
            assignActorToScene("17", 2, movie, positionScenario);
            assignActorToScene("16", 2, movie, positionScenario);
            assignActorToScene("15", 2, movie, positionScenario);
            assignActorToScene("14", 2, movie, positionScenario);

            createScene(9, true, "4", movie, positionScenario);
            assignActorToScene("40", 3, movie, positionScenario);
            assignActorToScene("39", 3, movie, positionScenario);
            assignActorToScene("38", 3, movie, positionScenario);
            assignActorToScene("37", 3, movie, positionScenario);

            createScene(8, true, "5", movie, positionScenario);
            assignActorToScene("40", 4, movie, positionScenario);
            assignActorToScene("39", 4, movie, positionScenario);
            assignActorToScene("38", 4, movie, positionScenario);
            assignActorToScene("37", 4, movie, positionScenario);

            createScene(12, false, "2", movie, positionScenario);
            assignActorToScene("6", 5, movie, positionScenario);
            assignActorToScene("17", 5, movie, positionScenario);
            assignActorToScene("16", 5, movie, positionScenario);
            assignActorToScene("15", 5, movie, positionScenario);

            createScene(20, false, "4", movie, positionScenario);
            assignActorToScene("6", 6, movie, positionScenario);
            assignActorToScene("7", 6, movie, positionScenario);
            assignActorToScene("8", 6, movie, positionScenario);
            assignActorToScene("10", 6, movie, positionScenario);

            createScene(9, false, "1", movie, positionScenario);
            assignActorToScene("12", 7, movie, positionScenario);
            assignActorToScene("15", 7, movie, positionScenario);
            assignActorToScene("16", 7, movie, positionScenario);
            assignActorToScene("17", 7, movie, positionScenario);

            createScene(15, false, "5", movie, positionScenario);
            assignActorToScene("12", 8, movie, positionScenario);
            assignActorToScene("39", 8, movie, positionScenario);
            assignActorToScene("38", 8, movie, positionScenario);
            assignActorToScene("37", 8, movie, positionScenario);

            createScene(21, false, "3", movie, positionScenario);
            assignActorToScene("1", 9, movie, positionScenario);
            assignActorToScene("2", 9, movie, positionScenario);
            assignActorToScene("4", 9, movie, positionScenario);
            assignActorToScene("5", 9, movie, positionScenario);
        }

        public static void createScenario2(int numberOfLocation, int numberOfActors, Movie movie, int positionScenario)
        {
            createCalendar(movie, positionScenario);
            createLocations(numberOfLocation, movie, positionScenario);
            createActors(numberOfActors, movie, positionScenario);

            //Escenario dos: Contendrá 15 escenas, 6 localidades, 60 actores disponibles, las relaciones de las escenas en este escenario serán:
            //Poseerán 3 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(23, true, "1", movie, positionScenario);//pages, shedule, idLocation, pelicula, scenario position 
            assignActorToScene("1", 0, movie, positionScenario);// idactor, positionScene, pelicula, posicion del escenario
            assignActorToScene("4", 0, movie, positionScenario);
            assignActorToScene("20", 0, movie, positionScenario);

            createScene(21, true, "6", movie, positionScenario);
            assignActorToScene("1", 1, movie, positionScenario);
            assignActorToScene("20", 1, movie, positionScenario);
            assignActorToScene("19", 1, movie, positionScenario);

            createScene(10, true, "5", movie, positionScenario);
            assignActorToScene("44", 2, movie, positionScenario);
            assignActorToScene("55", 2, movie, positionScenario);
            assignActorToScene("56", 2, movie, positionScenario);

            createScene(10, true, "3", movie, positionScenario);
            assignActorToScene("1", 3, movie, positionScenario);
            assignActorToScene("21", 3, movie, positionScenario);
            assignActorToScene("4", 3, movie, positionScenario);

            createScene(11, true, "4", movie, positionScenario);
            assignActorToScene("55", 4, movie, positionScenario);
            assignActorToScene("19", 4, movie, positionScenario);
            assignActorToScene("44", 4, movie, positionScenario);

            createScene(12, true, "2", movie, positionScenario);
            assignActorToScene("19", 5, movie, positionScenario);
            assignActorToScene("4", 5, movie, positionScenario);
            assignActorToScene("20", 5, movie, positionScenario);

            createScene(13, true, "1", movie, positionScenario);
            assignActorToScene("40", 6, movie, positionScenario);
            assignActorToScene("44", 6, movie, positionScenario);
            assignActorToScene("1", 6, movie, positionScenario);

            createScene(16, true, "2", movie, positionScenario);
            assignActorToScene("9", 7, movie, positionScenario);
            assignActorToScene("12", 7, movie, positionScenario);
            assignActorToScene("10", 7, movie, positionScenario);

            createScene(14, false, "3", movie, positionScenario);
            assignActorToScene("1", 8, movie, positionScenario);
            assignActorToScene("44", 8, movie, positionScenario);
            assignActorToScene("10", 8, movie, positionScenario);

            createScene(15, false, "4", movie, positionScenario);
            assignActorToScene("1", 9, movie, positionScenario);
            assignActorToScene("60", 9, movie, positionScenario);
            assignActorToScene("4", 9, movie, positionScenario);

            createScene(22, false, "6", movie, positionScenario);
            assignActorToScene("40", 10, movie, positionScenario);
            assignActorToScene("33", 10, movie, positionScenario);
            assignActorToScene("10", 10, movie, positionScenario);

            createScene(23, false, "6", movie, positionScenario);
            assignActorToScene("19", 11, movie, positionScenario);
            assignActorToScene("1", 11, movie, positionScenario);
            assignActorToScene("30", 11, movie, positionScenario);

            createScene(24, false, "5", movie, positionScenario);
            assignActorToScene("33", 12, movie, positionScenario);
            assignActorToScene("60", 12, movie, positionScenario);
            assignActorToScene("40", 12, movie, positionScenario);

            createScene(11, false, "4", movie, positionScenario);
            assignActorToScene("60", 13, movie, positionScenario);
            assignActorToScene("30", 13, movie, positionScenario);
            assignActorToScene("4", 13, movie, positionScenario);

            createScene(19, false, "3", movie, positionScenario);
            assignActorToScene("21", 14, movie, positionScenario);
            assignActorToScene("20", 14, movie, positionScenario);
            assignActorToScene("4", 14, movie, positionScenario);
        }

        public static void createScenario4(int numberOfLocation, int numberOfActors, Movie movie, int positionScenario)
        {
            createCalendar(movie, positionScenario);
            createLocations(numberOfLocation, movie, positionScenario);
            createActors(numberOfActors, movie, positionScenario);
            //Escenario cuatro: Contendrá 20 escenas, 8 localidades, 80 actores disponibles, las relaciones de las escenas en este escenario serán:
            //Poseerán 4 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(24, true, "1", movie, positionScenario);//pages, shedule, idLocation, pelicula, posicion del escenario
            assignActorToScene("80", 0, movie, positionScenario);// idactor, positionScene, pelicula, posicion del escenario
            assignActorToScene("70", 0, movie, positionScenario);
            assignActorToScene("60", 0, movie, positionScenario);
            assignActorToScene("61", 0, movie, positionScenario);

            createScene(24, true, "8", movie, positionScenario);
            assignActorToScene("10", 1, movie, positionScenario);
            assignActorToScene("4", 1, movie, positionScenario);
            assignActorToScene("11", 1, movie, positionScenario);
            assignActorToScene("80", 1, movie, positionScenario);

            createScene(14, true, "7", movie, positionScenario);
            assignActorToScene("60", 2, movie, positionScenario);
            assignActorToScene("3", 2, movie, positionScenario);
            assignActorToScene("61", 2, movie, positionScenario);
            assignActorToScene("6", 2, movie, positionScenario);

            createScene(16, true, "1", movie, positionScenario);
            assignActorToScene("80", 3, movie, positionScenario);
            assignActorToScene("77", 3, movie, positionScenario);
            assignActorToScene("60", 3, movie, positionScenario);
            assignActorToScene("19", 3, movie, positionScenario);

            createScene(19, true, "1", movie, positionScenario);
            assignActorToScene("20", 4, movie, positionScenario);
            assignActorToScene("55", 4, movie, positionScenario);
            assignActorToScene("4", 4, movie, positionScenario);
            assignActorToScene("10", 4, movie, positionScenario);

            createScene(18, true, "1", movie, positionScenario);
            assignActorToScene("61", 5, movie, positionScenario);
            assignActorToScene("77", 5, movie, positionScenario);
            assignActorToScene("55", 5, movie, positionScenario);
            assignActorToScene("11", 5, movie, positionScenario);

            createScene(23, true, "8", movie, positionScenario);
            assignActorToScene("77", 6, movie, positionScenario);
            assignActorToScene("16", 6, movie, positionScenario);
            assignActorToScene("60", 6, movie, positionScenario);
            assignActorToScene("33", 6, movie, positionScenario);

            createScene(11, true, "8", movie, positionScenario);
            assignActorToScene("22", 7, movie, positionScenario);
            assignActorToScene("24", 7, movie, positionScenario);
            assignActorToScene("77", 7, movie, positionScenario);
            assignActorToScene("74", 7, movie, positionScenario);

            createScene(14, true, "6", movie, positionScenario);
            assignActorToScene("10", 8, movie, positionScenario);
            assignActorToScene("30", 8, movie, positionScenario);
            assignActorToScene("32", 8, movie, positionScenario);
            assignActorToScene("55", 8, movie, positionScenario);

            createScene(15, true, "4", movie, positionScenario);
            assignActorToScene("4", 9, movie, positionScenario);
            assignActorToScene("19", 9, movie, positionScenario);
            assignActorToScene("16", 9, movie, positionScenario);
            assignActorToScene("77", 9, movie, positionScenario);

            createScene(16, false, "3", movie, positionScenario);
            assignActorToScene("10", 10, movie, positionScenario);
            assignActorToScene("12", 10, movie, positionScenario);
            assignActorToScene("11", 10, movie, positionScenario);
            assignActorToScene("3", 10, movie, positionScenario);

            createScene(20, false, "1", movie, positionScenario);
            assignActorToScene("14", 11, movie, positionScenario);
            assignActorToScene("13", 11, movie, positionScenario);
            assignActorToScene("79", 11, movie, positionScenario);
            assignActorToScene("15", 11, movie, positionScenario);

            createScene(19, false, "3", movie, positionScenario);
            assignActorToScene("13", 12, movie, positionScenario);
            assignActorToScene("14", 12, movie, positionScenario);
            assignActorToScene("79", 12, movie, positionScenario);
            assignActorToScene("44", 12, movie, positionScenario);

            createScene(25, false, "5", movie, positionScenario);
            assignActorToScene("14", 13, movie, positionScenario);
            assignActorToScene("78", 13, movie, positionScenario);
            assignActorToScene("15", 13, movie, positionScenario);
            assignActorToScene("79", 13, movie, positionScenario);

            createScene(22, false, "6", movie, positionScenario);
            assignActorToScene("8", 14, movie, positionScenario);
            assignActorToScene("7", 14, movie, positionScenario);
            assignActorToScene("9", 14, movie, positionScenario);
            assignActorToScene("55", 14, movie, positionScenario);

            createScene(14, false, "6", movie, positionScenario);
            assignActorToScene("3", 15, movie, positionScenario);
            assignActorToScene("79", 15, movie, positionScenario);
            assignActorToScene("15", 15, movie, positionScenario);
            assignActorToScene("14", 15, movie, positionScenario);

            createScene(19, false, "7", movie, positionScenario);
            assignActorToScene("15", 16, movie, positionScenario);
            assignActorToScene("33", 16, movie, positionScenario);
            assignActorToScene("2", 16, movie, positionScenario);
            assignActorToScene("1", 16, movie, positionScenario);

            createScene(18, false, "2", movie, positionScenario);
            assignActorToScene("33", 17, movie, positionScenario);
            assignActorToScene("55", 17, movie, positionScenario);
            assignActorToScene("78", 17, movie, positionScenario);
            assignActorToScene("22", 17, movie, positionScenario);

            createScene(17, false, "8", movie, positionScenario);
            assignActorToScene("79", 18, movie, positionScenario);
            assignActorToScene("19", 18, movie, positionScenario);
            assignActorToScene("77", 18, movie, positionScenario);
            assignActorToScene("12", 18, movie, positionScenario);

            createScene(18, false, "3", movie, positionScenario);
            assignActorToScene("78", 19, movie, positionScenario);
            assignActorToScene("34", 19, movie, positionScenario);
            assignActorToScene("64", 19, movie, positionScenario);
            assignActorToScene("12", 19, movie, positionScenario);;
        }

        public static void createScenario3(int numberOfLocation, int numberOfActors, Movie movie, int positionScenario)
        {
            createCalendar(movie, positionScenario);
            createLocations(numberOfLocation, movie, positionScenario);
            createActors(numberOfActors, movie, positionScenario);

            //Escenario tres: Contendrá 17 escenas, 7 localidades, 100 actores disponibles, las relaciones de las escenas en este escenario serán:
            //Poseerán 5 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(13, true, "1", movie, positionScenario);//pages, shedule, idLocation, , pelicula, posicion del escenario
            assignActorToScene("1", 0, movie, positionScenario);// idactor, positionScene, pelicula, posicion del escenario
            assignActorToScene("100", 0, movie, positionScenario);
            assignActorToScene("80", 0, movie, positionScenario);
            assignActorToScene("8", 0, movie, positionScenario);
            assignActorToScene("7", 0, movie, positionScenario);

            createScene(20, true, "2", movie, positionScenario);
            assignActorToScene("5", 1, movie, positionScenario);
            assignActorToScene("1", 1, movie, positionScenario);
            assignActorToScene("67", 1, movie, positionScenario);
            assignActorToScene("34", 1, movie, positionScenario);
            assignActorToScene("30", 1, movie, positionScenario);

            createScene(24, true, "7", movie, positionScenario);
            assignActorToScene("20", 2, movie, positionScenario);
            assignActorToScene("8", 2, movie, positionScenario);
            assignActorToScene("99", 2, movie, positionScenario);
            assignActorToScene("1", 2, movie, positionScenario);
            assignActorToScene("30", 2, movie, positionScenario);

            createScene(11, true, "7", movie, positionScenario);
            assignActorToScene("100", 3, movie, positionScenario);
            assignActorToScene("22", 3, movie, positionScenario);
            assignActorToScene("7", 3, movie, positionScenario);
            assignActorToScene("35", 3, movie, positionScenario);
            assignActorToScene("94", 3, movie, positionScenario);

            createScene(18, true, "6", movie, positionScenario);
            assignActorToScene("14", 4, movie, positionScenario);
            assignActorToScene("40", 4, movie, positionScenario);
            assignActorToScene("20", 4, movie, positionScenario);
            assignActorToScene("69", 4, movie, positionScenario);
            assignActorToScene("68", 4, movie, positionScenario);

            createScene(13, true, "7", movie, positionScenario);
            assignActorToScene("78", 5, movie, positionScenario);
            assignActorToScene("58", 5, movie, positionScenario);
            assignActorToScene("29", 5, movie, positionScenario);
            assignActorToScene("2", 5, movie, positionScenario);
            assignActorToScene("100", 5, movie, positionScenario);

            createScene(19, true, "4", movie, positionScenario);
            assignActorToScene("78", 6, movie, positionScenario);
            assignActorToScene("28", 6, movie, positionScenario);
            assignActorToScene("20", 6, movie, positionScenario);
            assignActorToScene("30", 6, movie, positionScenario);
            assignActorToScene("40", 6, movie, positionScenario);

            createScene(17, true, "3", movie, positionScenario);
            assignActorToScene("20", 7, movie, positionScenario);
            assignActorToScene("8", 7, movie, positionScenario);
            assignActorToScene("99", 7, movie, positionScenario);
            assignActorToScene("78", 7, movie, positionScenario);
            assignActorToScene("29", 7, movie, positionScenario);

            createScene(16, true, "3", movie, positionScenario);
            assignActorToScene("58", 8, movie, positionScenario);
            assignActorToScene("1", 8, movie, positionScenario);
            assignActorToScene("8", 8, movie, positionScenario);
            assignActorToScene("22", 8, movie, positionScenario);
            assignActorToScene("78", 8, movie, positionScenario);

            createScene(21, false, "7", movie, positionScenario);
            assignActorToScene("10", 9, movie, positionScenario);
            assignActorToScene("14", 9, movie, positionScenario);
            assignActorToScene("4", 9, movie, positionScenario);
            assignActorToScene("58", 9, movie, positionScenario);
            assignActorToScene("78", 9, movie, positionScenario);

            createScene(17, false, "4", movie, positionScenario);
            assignActorToScene("98", 10, movie, positionScenario);
            assignActorToScene("65", 10, movie, positionScenario);
            assignActorToScene("1", 10, movie, positionScenario);
            assignActorToScene("78", 10, movie, positionScenario);
            assignActorToScene("28", 10, movie, positionScenario);

            createScene(24, false, "7", movie, positionScenario);
            assignActorToScene("13", 11, movie, positionScenario);
            assignActorToScene("79", 11, movie, positionScenario);
            assignActorToScene("35", 11, movie, positionScenario);
            assignActorToScene("18", 11, movie, positionScenario);
            assignActorToScene("10", 11, movie, positionScenario);

            createScene(22, false, "4", movie, positionScenario);
            assignActorToScene("99", 12, movie, positionScenario);
            assignActorToScene("3", 12, movie, positionScenario);
            assignActorToScene("30", 12, movie, positionScenario);
            assignActorToScene("100", 12, movie, positionScenario);
            assignActorToScene("4", 12, movie, positionScenario);

            createScene(23, false, "5", movie, positionScenario);
            assignActorToScene("98", 13, movie, positionScenario);
            assignActorToScene("65", 13, movie, positionScenario);
            assignActorToScene("24", 13, movie, positionScenario);
            assignActorToScene("10", 13, movie, positionScenario);
            assignActorToScene("14", 13, movie, positionScenario);

            createScene(18, false, "6", movie, positionScenario);
            assignActorToScene("17", 14, movie, positionScenario);
            assignActorToScene("3", 14, movie, positionScenario);
            assignActorToScene("2", 14, movie, positionScenario);
            assignActorToScene("10", 14, movie, positionScenario);
            assignActorToScene("11", 14, movie, positionScenario);

            createScene(17, false, "2", movie, positionScenario);
            assignActorToScene("7", 15, movie, positionScenario);
            assignActorToScene("8", 15, movie, positionScenario);
            assignActorToScene("3", 15, movie, positionScenario);
            assignActorToScene("4", 15, movie, positionScenario);
            assignActorToScene("14", 15, movie, positionScenario);

            createScene(15, false, "1", movie, positionScenario);
            assignActorToScene("67", 16, movie, positionScenario);
            assignActorToScene("68", 16, movie, positionScenario);
            assignActorToScene("66", 16, movie, positionScenario);
            assignActorToScene("2", 16, movie, positionScenario);
            assignActorToScene("1", 16, movie, positionScenario);
        }

        public static void firstAndLastDaysActors() {// Este metodo calcula cual es el primero y el ultimo dia de participación de un actor 


        }

       

        public static void printScenarios(Movie movie) {
            Console.WriteLine("Numero de scenarios " + movie.Scenarios.Count + "\n");
            int scenariosNumber = 1;
            for (int i=0; i< movie.Scenarios.Count;i++) {  
                    Console.WriteLine("      Scenario number " + scenariosNumber + "      Number of scenes " + movie.Scenarios[i].FilmingCalendars[0].Scenes.Count);
                scenariosNumber += 1;
                foreach (Scene scene in movie.Scenarios[i].FilmingCalendars[0].Scenes)
                    {
                        Console.WriteLine("--------------------------------------------------------------------------------------");
                        Console.WriteLine("                                       Escene                                        ");
                        Console.WriteLine("--------------------------------------------------------------------------------------");
                        Console.WriteLine("  Duration: " + scene.Duration + "   Pages: " + scene.Pages +
                        "    Schedule: " + scene.Schedule + "   Location: " + scene.Location.ID);
                        Console.WriteLine("---------------------------------------Actors-----------------------------------------");
                        foreach (Actor actor in scene.Actors)
                        {
                            Console.WriteLine("Id actor: " + actor.ID + "    Coste per day: " + actor.CostPerDay + "    First participation: " + actor.FirstParticipation +
                            "    Last participation: " + actor.LastParticipation + "\n");
                        }
                    }
                    Console.WriteLine("\n\n\n");
            }  
        }
    }
}

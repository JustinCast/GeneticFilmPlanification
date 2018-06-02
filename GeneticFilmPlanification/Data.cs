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
        static Movie movie = Movie.GetInstance();

        public static void assignLocationsToDay() {// Por cada escenario recorre la lista de las localizaciones y las va agregando a la lista de disponibles marcando cuales estan en uso y cuales no en la jornada ya sea dia o noche 
            for (int i=0; i< movie.Scenarios.Count;i++) {
                foreach (Day day in movie.Scenarios[i].Days) {
                    foreach (Location location in movie.Scenarios[i].Locations ) {
                        if (day.DayTime.Scenes.Count > 0)
                        {
                            foreach (Scene scene in day.DayTime.Scenes)
                            {
                                Location auxLocation = location;
                                if (auxLocation.ID==scene.Location.ID) {
                                    auxLocation.InUse = true;
                                    day.DayTime.AvailableLocations.Add(auxLocation);
                                }
                                day.DayTime.AvailableLocations.Add(location);  
                            }
                        }
                        if(day.NightTime.Scenes.Count > 0) {
                            foreach (Scene scene in day.NightTime.Scenes)
                            {
                                Location auxLocation = location;
                                if (auxLocation.ID == scene.Location.ID)
                                {
                                    auxLocation.InUse = true;
                                    day.NightTime.AvailableLocations.Add(auxLocation);
                                }
                                day.NightTime.AvailableLocations.Add(location);
                            }
                        }    
                    }
                }
            }
        }

        public static void createDays()
        {// crea los dias ademas de agregar los objetos de jornada dia y jornada noche con un maximo de paginas de 35
            foreach(Scenario scenario in  movie.Scenarios) {
                int quantity = 0;
                int position = movie.Scenarios.IndexOf(scenario);
                if (position == 0)
                    quantity = 10;
                if (position==1)
                    quantity = 15;
                if (position == 2)
                    quantity = 17;
                if (position == 3)
                    quantity = 20;
                for (int i = 1; i <= quantity; i++)
                {
                    Day newDay = new Day();
                    newDay.DayNumber = i;
                    Time time1 = new Time();
                    time1.MaximunScriptPages = 45;
                    Time time2 = new Time();
                    time2.MaximunScriptPages = 45;
                    newDay.DayTime = time1;
                    newDay.NightTime = time2;
                    scenario.Days.Add(newDay);
                }
            }  
        }

        public static void assignScenesToDay(int positionScenario) {// Este metodo recorre los dias y le asigna una escena ya sea de dia o de noche 
            foreach (Day day in movie.Scenarios[positionScenario].Days) {
                foreach (Scene scene in movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes)
                {
                    if (scene.assigned == false) {
                        if (scene.Schedule == true)
                        {
                            scene.assigned = true;
                            day.DayTime.Scenes.Add(scene);
                            break;
                        }
                        if (scene.Schedule == false)
                        {
                            scene.assigned = true;
                            day.NightTime.Scenes.Add(scene);
                            break;
                        }
                    }
                }
            }
        } 

        public static void printDays() {
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

        public static void createScenariosOfMovie() {
            for (int i=0; i<4;i++) {
                Scenario newScenario = new Scenario();
                movie.Scenarios.Add(newScenario);
            }
        }

        public static void createActors(int numberOfActors, int positionScenario)
        {
            for (int i = 1; i <= numberOfActors; i++)
            {
                Actor newActor = new Actor();
                newActor.CostPerDay = i+2;
                newActor.ID = i.ToString();
                movie.Scenarios[positionScenario].Actors.Add(newActor);
            }
        }

        public static void createLocations(int numberOfLocations, int positionScenario)
        {
            for (int i = 1; i <= numberOfLocations; i++)
            {
                Location newLocation = new Location();
                newLocation.ID = i.ToString();
                newLocation.InUse = false;
                movie.Scenarios[positionScenario].Locations.Add(newLocation);
            }
        }


        public static void createCalendar(int positionScenario) {
            FilmingCalendar newCalendar = new FilmingCalendar();
            movie.Scenarios[positionScenario].FilmingCalendars.Add(newCalendar);
        }

        public static Location searchLocation(string idLocation, int positionScenario)
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

        public static void createScene(int pages, bool shedule,string idLocation, int positionScenario, String id  ) {
            Scene newScene = new Scene();
            Location currentLocation = searchLocation(idLocation,positionScenario);
            newScene.Location = currentLocation;
            currentLocation.InUse = true;
            newScene.Pages = pages;
            newScene.Schedule = shedule;
            newScene.id = id;
            movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes.Add(newScene);
        }

        public static Actor searchActor(string idActor, int positionScenario)
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

        public static void assignActorToScene(string idActor,int positionScene, int positionScenario) {
            Actor actor = searchActor(idActor, positionScenario);
            movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes[positionScene].Actors.Add(actor);
        }
 

        public static void createScenario1(int numberOfLocation, int numberOfActors, int positionScenario)
        {
            createCalendar( positionScenario);
            createLocations(numberOfLocation, positionScenario);
            createActors(numberOfActors, positionScenario);
            // Contendrá 10 escenas, 5 localidades,40 actores disponibles, 
            //las relaciones de las escenas en este escenario serán:
            //Poseerán 4 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(10, true,"1", positionScenario,"1");//pages, shedule, idLocation, posicion del escenario,id
            assignActorToScene("1",0, positionScenario);// idactor, positionScene, posicion del escenario
            assignActorToScene("2", 0, positionScenario);
            assignActorToScene("4", 0, positionScenario);
            assignActorToScene("5", 0, positionScenario);

            createScene(12, true, "2", positionScenario, "2");
            assignActorToScene("6", 1, positionScenario);
            assignActorToScene("7", 1, positionScenario);
            assignActorToScene("8", 1, positionScenario);
            assignActorToScene("9", 1, positionScenario);

            createScene(15, true, "3", positionScenario, "3");
            assignActorToScene("17", 2, positionScenario);
            assignActorToScene("16", 2, positionScenario);
            assignActorToScene("15", 2, positionScenario);
            assignActorToScene("14", 2, positionScenario);

            createScene(9, true, "4", positionScenario, "4");
            assignActorToScene("40", 3, positionScenario);
            assignActorToScene("39", 3, positionScenario);
            assignActorToScene("38", 3, positionScenario);
            assignActorToScene("37", 3, positionScenario);

            createScene(8, true, "5", positionScenario, "5");
            assignActorToScene("40", 4, positionScenario);
            assignActorToScene("39", 4, positionScenario);
            assignActorToScene("38", 4, positionScenario);
            assignActorToScene("37", 4, positionScenario);

            createScene(12, false, "2", positionScenario, "6");
            assignActorToScene("6", 5, positionScenario);
            assignActorToScene("17", 5, positionScenario);
            assignActorToScene("16", 5, positionScenario);
            assignActorToScene("15", 5, positionScenario);

            createScene(20, false, "4", positionScenario, "7");
            assignActorToScene("6", 6, positionScenario);
            assignActorToScene("7", 6, positionScenario);
            assignActorToScene("8", 6, positionScenario);
            assignActorToScene("10", 6, positionScenario);

            createScene(9, false, "1", positionScenario, "8");
            assignActorToScene("12", 7, positionScenario);
            assignActorToScene("15", 7, positionScenario);
            assignActorToScene("16", 7, positionScenario);
            assignActorToScene("17", 7, positionScenario);

            createScene(15, false, "5", positionScenario, "9");
            assignActorToScene("12", 8, positionScenario);
            assignActorToScene("39", 8, positionScenario);
            assignActorToScene("38", 8, positionScenario);
            assignActorToScene("37", 8, positionScenario);

            createScene(21, false, "3", positionScenario, "10");
            assignActorToScene("1", 9, positionScenario);
            assignActorToScene("2", 9, positionScenario);
            assignActorToScene("4", 9, positionScenario);
            assignActorToScene("5", 9, positionScenario);
        }

        public static void createScenario2(int numberOfLocation, int numberOfActors, int positionScenario)
        {
            createCalendar(positionScenario);
            createLocations(numberOfLocation, positionScenario);
            createActors(numberOfActors, positionScenario);

            //Escenario dos: Contendrá 15 escenas, 6 localidades, 60 actores disponibles, las relaciones de las escenas en este escenario serán:
            //Poseerán 3 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(23, true, "1", positionScenario, "1");//pages, shedule, idLocation, scenario position ,id
            assignActorToScene("1", 0, positionScenario);// idactor, positionScene, posicion del escenario
            assignActorToScene("4", 0, positionScenario);
            assignActorToScene("20", 0, positionScenario);

            createScene(21, true, "6", positionScenario, "2");
            assignActorToScene("1", 1, positionScenario);
            assignActorToScene("20", 1, positionScenario);
            assignActorToScene("19", 1, positionScenario);

            createScene(10, true, "5", positionScenario, "3");
            assignActorToScene("44", 2, positionScenario);
            assignActorToScene("55", 2, positionScenario);
            assignActorToScene("56", 2, positionScenario);

            createScene(10, true, "3", positionScenario, "4");
            assignActorToScene("1", 3, positionScenario);
            assignActorToScene("21", 3, positionScenario);
            assignActorToScene("4", 3, positionScenario);

            createScene(11, true, "4", positionScenario, "5");
            assignActorToScene("55", 4, positionScenario);
            assignActorToScene("19", 4, positionScenario);
            assignActorToScene("44", 4, positionScenario);

            createScene(12, true, "2", positionScenario, "6");
            assignActorToScene("19", 5, positionScenario);
            assignActorToScene("4", 5, positionScenario);
            assignActorToScene("20", 5, positionScenario);

            createScene(13, true, "1", positionScenario, "7");
            assignActorToScene("40", 6, positionScenario);
            assignActorToScene("44", 6, positionScenario);
            assignActorToScene("1", 6, positionScenario);

            createScene(16, true, "2", positionScenario, "8");
            assignActorToScene("9", 7, positionScenario);
            assignActorToScene("12", 7, positionScenario);
            assignActorToScene("10", 7, positionScenario);

            createScene(14, false, "3", positionScenario, "9");
            assignActorToScene("1", 8, positionScenario);
            assignActorToScene("44", 8, positionScenario);
            assignActorToScene("10", 8, positionScenario);

            createScene(15, false, "4", positionScenario, "10");
            assignActorToScene("1", 9, positionScenario);
            assignActorToScene("60", 9, positionScenario);
            assignActorToScene("4", 9, positionScenario);

            createScene(22, false, "6", positionScenario, "11");
            assignActorToScene("40", 10, positionScenario);
            assignActorToScene("33", 10, positionScenario);
            assignActorToScene("10", 10, positionScenario);

            createScene(23, false, "6", positionScenario, "12");
            assignActorToScene("19", 11, positionScenario);
            assignActorToScene("1", 11, positionScenario);
            assignActorToScene("30", 11, positionScenario);

            createScene(24, false, "5", positionScenario, "13");
            assignActorToScene("33", 12, positionScenario);
            assignActorToScene("60", 12, positionScenario);
            assignActorToScene("40", 12, positionScenario);

            createScene(11, false, "4", positionScenario, "14");
            assignActorToScene("60", 13, positionScenario);
            assignActorToScene("30", 13, positionScenario);
            assignActorToScene("4", 13, positionScenario);

            createScene(19, false, "3", positionScenario, "15");
            assignActorToScene("21", 14, positionScenario);
            assignActorToScene("20", 14, positionScenario);
            assignActorToScene("4", 14, positionScenario);
        }

        public static void createScenario4(int numberOfLocation, int numberOfActors, int positionScenario)
        {
            createCalendar(positionScenario);
            createLocations(numberOfLocation, positionScenario);
            createActors(numberOfActors, positionScenario);
            //Escenario cuatro: Contendrá 20 escenas, 8 localidades, 80 actores disponibles, las relaciones de las escenas en este escenario serán:
            //Poseerán 4 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(24, true, "1", positionScenario, "1");//pages, shedule, idLocation, pelicula, posicion del escenario,id
            assignActorToScene("80", 0, positionScenario);// idactor, positionScene, pelicula, posicion del escenario
            assignActorToScene("70", 0, positionScenario);
            assignActorToScene("60", 0, positionScenario);
            assignActorToScene("61", 0, positionScenario);

            createScene(24, true, "8", positionScenario, "2");
            assignActorToScene("10", 1, positionScenario);
            assignActorToScene("4", 1, positionScenario);
            assignActorToScene("11", 1, positionScenario);
            assignActorToScene("80", 1, positionScenario);

            createScene(14, true, "7", positionScenario, "3");
            assignActorToScene("60", 2, positionScenario);
            assignActorToScene("3", 2, positionScenario);
            assignActorToScene("61", 2, positionScenario);
            assignActorToScene("6", 2, positionScenario);

            createScene(16, true, "1", positionScenario, "4");
            assignActorToScene("80", 3, positionScenario);
            assignActorToScene("77", 3, positionScenario);
            assignActorToScene("60", 3, positionScenario);
            assignActorToScene("19", 3, positionScenario);

            createScene(19, true, "1", positionScenario, "5");
            assignActorToScene("20", 4, positionScenario);
            assignActorToScene("55", 4, positionScenario);
            assignActorToScene("4", 4, positionScenario);
            assignActorToScene("10", 4, positionScenario);

            createScene(18, true, "1", positionScenario, "6");
            assignActorToScene("61", 5, positionScenario);
            assignActorToScene("77", 5, positionScenario);
            assignActorToScene("55", 5, positionScenario);
            assignActorToScene("11", 5, positionScenario);

            createScene(23, true, "8", positionScenario, "7");
            assignActorToScene("77", 6, positionScenario);
            assignActorToScene("16", 6, positionScenario);
            assignActorToScene("60", 6, positionScenario);
            assignActorToScene("33", 6, positionScenario);

            createScene(11, true, "8", positionScenario, "8");
            assignActorToScene("22", 7, positionScenario);
            assignActorToScene("24", 7, positionScenario);
            assignActorToScene("77", 7, positionScenario);
            assignActorToScene("74", 7, positionScenario);

            createScene(14, true, "6", positionScenario, "9");
            assignActorToScene("10", 8, positionScenario);
            assignActorToScene("30", 8, positionScenario);
            assignActorToScene("32", 8, positionScenario);
            assignActorToScene("55", 8, positionScenario);

            createScene(15, true, "4", positionScenario, "10");
            assignActorToScene("4", 9, positionScenario);
            assignActorToScene("19", 9, positionScenario);
            assignActorToScene("16", 9, positionScenario);
            assignActorToScene("77", 9, positionScenario);

            createScene(16, false, "3", positionScenario, "11");
            assignActorToScene("10", 10, positionScenario);
            assignActorToScene("12", 10, positionScenario);
            assignActorToScene("11", 10, positionScenario);
            assignActorToScene("3", 10, positionScenario);

            createScene(20, false, "1", positionScenario, "12");
            assignActorToScene("14", 11, positionScenario);
            assignActorToScene("13", 11, positionScenario);
            assignActorToScene("79", 11, positionScenario);
            assignActorToScene("15", 11, positionScenario);

            createScene(19, false, "3", positionScenario, "13");
            assignActorToScene("13", 12, positionScenario);
            assignActorToScene("14", 12, positionScenario);
            assignActorToScene("79", 12, positionScenario);
            assignActorToScene("44", 12, positionScenario);

            createScene(25, false, "5", positionScenario, "14");
            assignActorToScene("14", 13, positionScenario);
            assignActorToScene("78", 13, positionScenario);
            assignActorToScene("15", 13, positionScenario);
            assignActorToScene("79", 13, positionScenario);

            createScene(22, false, "6", positionScenario, "15");
            assignActorToScene("8", 14, positionScenario);
            assignActorToScene("7", 14, positionScenario);
            assignActorToScene("9", 14, positionScenario);
            assignActorToScene("55", 14, positionScenario);

            createScene(14, false, "6", positionScenario, "16");
            assignActorToScene("3", 15, positionScenario);
            assignActorToScene("79", 15, positionScenario);
            assignActorToScene("15", 15, positionScenario);
            assignActorToScene("14", 15, positionScenario);

            createScene(19, false, "7", positionScenario, "17");
            assignActorToScene("15", 16, positionScenario);
            assignActorToScene("33", 16, positionScenario);
            assignActorToScene("2", 16, positionScenario);
            assignActorToScene("1", 16, positionScenario);

            createScene(18, false, "2", positionScenario, "18");
            assignActorToScene("33", 17, positionScenario);
            assignActorToScene("55", 17, positionScenario);
            assignActorToScene("78", 17, positionScenario);
            assignActorToScene("22", 17, positionScenario);

            createScene(17, false, "8", positionScenario, "19");
            assignActorToScene("79", 18, positionScenario);
            assignActorToScene("19", 18, positionScenario);
            assignActorToScene("77", 18, positionScenario);
            assignActorToScene("12", 18, positionScenario);

            createScene(18, false, "3", positionScenario, "20");
            assignActorToScene("78", 19, positionScenario);
            assignActorToScene("34", 19, positionScenario);
            assignActorToScene("64", 19, positionScenario);
            assignActorToScene("12", 19, positionScenario);;
        }

        public static void createScenario3(int numberOfLocation, int numberOfActors, int positionScenario)
        {
            createCalendar(positionScenario);
            createLocations(numberOfLocation,positionScenario);
            createActors(numberOfActors, positionScenario);

            //Escenario tres: Contendrá 17 escenas, 7 localidades, 100 actores disponibles, las relaciones de las escenas en este escenario serán:
            //Poseerán 5 actores cada una, 1 localidad, 1 rol de día y 1 de noche.

            createScene(13, true, "1",positionScenario,"1");//pages, shedule, idLocation , posicion del escenario,id
            assignActorToScene("1", 0, positionScenario);// idactor, positionScene, posicion del escenario
            assignActorToScene("100", 0, positionScenario);
            assignActorToScene("80", 0, positionScenario);
            assignActorToScene("8", 0, positionScenario);
            assignActorToScene("7", 0, positionScenario);

            createScene(20, true, "2", positionScenario, "2");
            assignActorToScene("5", 1, positionScenario);
            assignActorToScene("1", 1, positionScenario);
            assignActorToScene("67", 1, positionScenario);
            assignActorToScene("34", 1, positionScenario);
            assignActorToScene("30", 1, positionScenario);

            createScene(24, true, "7", positionScenario, "3");
            assignActorToScene("20", 2, positionScenario);
            assignActorToScene("8", 2, positionScenario);
            assignActorToScene("99", 2, positionScenario);
            assignActorToScene("1", 2, positionScenario);
            assignActorToScene("30", 2, positionScenario);

            createScene(11, true, "7", positionScenario, "4");
            assignActorToScene("100", 3, positionScenario);
            assignActorToScene("22", 3, positionScenario);
            assignActorToScene("7", 3, positionScenario);
            assignActorToScene("35", 3, positionScenario);
            assignActorToScene("94", 3, positionScenario);

            createScene(18, true, "6", positionScenario, "5");
            assignActorToScene("14", 4, positionScenario);
            assignActorToScene("40", 4, positionScenario);
            assignActorToScene("20", 4, positionScenario);
            assignActorToScene("69", 4, positionScenario);
            assignActorToScene("68", 4, positionScenario);

            createScene(13, true, "7", positionScenario, "6");
            assignActorToScene("78", 5, positionScenario);
            assignActorToScene("58", 5, positionScenario);
            assignActorToScene("29", 5, positionScenario);
            assignActorToScene("2", 5, positionScenario);
            assignActorToScene("100", 5, positionScenario);

            createScene(19, true, "4", positionScenario, "7");
            assignActorToScene("78", 6, positionScenario);
            assignActorToScene("28", 6, positionScenario);
            assignActorToScene("20", 6, positionScenario);
            assignActorToScene("30", 6, positionScenario);
            assignActorToScene("40", 6, positionScenario);

            createScene(17, true, "3", positionScenario, "8");
            assignActorToScene("20", 7, positionScenario);
            assignActorToScene("8", 7, positionScenario);
            assignActorToScene("99", 7, positionScenario);
            assignActorToScene("78", 7, positionScenario);
            assignActorToScene("29", 7, positionScenario);

            createScene(16, true, "3", positionScenario, "9");
            assignActorToScene("58", 8, positionScenario);
            assignActorToScene("1", 8, positionScenario);
            assignActorToScene("8", 8, positionScenario);
            assignActorToScene("22", 8, positionScenario);
            assignActorToScene("78", 8, positionScenario);

            createScene(21, false, "7", positionScenario, "10");
            assignActorToScene("10", 9, positionScenario);
            assignActorToScene("14", 9, positionScenario);
            assignActorToScene("4", 9, positionScenario);
            assignActorToScene("58", 9, positionScenario);
            assignActorToScene("78", 9, positionScenario);

            createScene(17, false, "4", positionScenario, "11");
            assignActorToScene("98", 10, positionScenario);
            assignActorToScene("65", 10, positionScenario);
            assignActorToScene("1", 10, positionScenario);
            assignActorToScene("78", 10, positionScenario);
            assignActorToScene("28", 10, positionScenario);

            createScene(24, false, "7", positionScenario, "12");
            assignActorToScene("13", 11, positionScenario);
            assignActorToScene("79", 11, positionScenario);
            assignActorToScene("35", 11, positionScenario);
            assignActorToScene("18", 11, positionScenario);
            assignActorToScene("10", 11, positionScenario);

            createScene(22, false, "4", positionScenario, "13");
            assignActorToScene("99", 12, positionScenario);
            assignActorToScene("3", 12, positionScenario);
            assignActorToScene("30", 12, positionScenario);
            assignActorToScene("100", 12, positionScenario);
            assignActorToScene("4", 12, positionScenario);

            createScene(23, false, "5", positionScenario, "14");
            assignActorToScene("98", 13, positionScenario);
            assignActorToScene("24", 13,positionScenario);
            assignActorToScene("10", 13, positionScenario);
            assignActorToScene("14", 13, positionScenario);

            createScene(18, false, "6", positionScenario, "15");
            assignActorToScene("17", 14, positionScenario);
            assignActorToScene("3", 14, positionScenario);
            assignActorToScene("2", 14, positionScenario);
            assignActorToScene("10", 14, positionScenario);
            assignActorToScene("11", 14, positionScenario);

            createScene(17, false, "2", positionScenario, "16");
            assignActorToScene("7", 15, positionScenario);
            assignActorToScene("8", 15, positionScenario);
            assignActorToScene("3", 15, positionScenario);
            assignActorToScene("4", 15, positionScenario);
            assignActorToScene("14", 15, positionScenario);

            createScene(15, false, "1", positionScenario, "17");
            assignActorToScene("67", 16, positionScenario);
            assignActorToScene("68", 16, positionScenario);
            assignActorToScene("66", 16, positionScenario);
            assignActorToScene("2", 16, positionScenario);
            assignActorToScene("1", 16, positionScenario);
        }

        public static void printScenarios() {
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
                            Console.WriteLine("Id actor: " + actor.ID + "    Coste per day: " + actor.CostPerDay+ "\n");
                        }
                    }
                    Console.WriteLine("\n\n\n");
            }  
        }

        public static int calculatePriceOfCalendar(int positionScenario,List<Day> days) {// calcula el precio total del calendario en los dias 
            int totalPrice = 0;
            List<Actor> actors = movie.Scenarios[positionScenario].Actors;
            int firstParticipation = 0;
            int lastParticipation = 0;
            foreach (Actor actor in actors)
            {
                foreach (Day day in days)
                {
                    if (day.DayTime.Scenes.Count != 0)// recocrre las escenas de dia unicamente 
                    {
                        foreach (Scene scene in day.DayTime.Scenes)
                        {
                            foreach (Actor auxActor in scene.Actors)
                            {
                                if (auxActor.ID.Equals(actor.ID) && firstParticipation==0) {
                                    firstParticipation = day.DayNumber;
                                    break;
                                }
                                if (auxActor.ID.Equals(actor.ID))
                                {
                                    lastParticipation = day.DayNumber;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    if (day.NightTime.Scenes.Count != 0)// recorre las escenas de noche unicamente 
                    {
                        foreach (Scene scene in day.NightTime.Scenes)
                        {
                            foreach (Actor auxActor in scene.Actors)
                            {
                                if (auxActor.ID.Equals(actor.ID) && firstParticipation == 0)
                                {
                                    firstParticipation = day.DayNumber;
                                    break;
                                }
                                if (auxActor.ID.Equals(actor.ID))
                                {
                                    lastParticipation = day.DayNumber;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                for (int i = firstParticipation; i <= lastParticipation; i++)// suma el precio del actor desde el primer hasta el ultimo dia                                                             
                    totalPrice += actor.CostPerDay;                          // que aparece en el calendario  y se lo suma al coste total del calendario 
                if (firstParticipation!=0 && lastParticipation==0) {
                    totalPrice += actor.CostPerDay;
                }
                //Console.WriteLine("\n");
                //Console.WriteLine("Actor: "+ actor.ID+" First participation: day"+firstParticipation+"  last participation: day"+lastParticipation+"  CostPerDay: "+actor.CostPerDay);
                firstParticipation = 0;
                lastParticipation = 0;
            }
            //Console.WriteLine("valor del calendario ....................................... "+totalPrice);
            return totalPrice;
        }

        public static void performPmxInAllScenarios() {
            for (int i= 0;i < 1; i++) {
                Pmx.performCrossingPMX(i);// posicion del escenario
                int numberOfScenario = i + 1;      
                List<Day> days = Pmx.chooseTheBestCalendar(i);// posición del escenario    
                int costo1 = Data.calculatePriceOfCalendar(i, movie.Scenarios[i].Days);
                int costo2 = Data.calculatePriceOfCalendar(i, days);
                Console.WriteLine("                         Escenario numero " + numberOfScenario);
                Console.WriteLine("calendario original el costo es de : " + costo1 + " calendario mutado el costo es de : " + costo2);
            }
            
        }
    }
}

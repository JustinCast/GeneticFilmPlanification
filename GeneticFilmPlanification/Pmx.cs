using GeneticFilmPlanification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification
{
    class Pmx
    {
        static int countC = 0;// contador de comparaciones
        static int countA = 0;// contador de asignaciones
        static Movie movie = Movie.GetInstance();

        /*------------------------------------------------------------------------------------------------------------------------------
                                                  ALGORITMO GENETICO PMX
        --------------------------------------------------------------------------------------------------------------------------------
        */

        public static FilmingCalendar generateChromosome(FilmingCalendar calendar)
        {// Se recorre las escenas de un calendario y se le van agregando inversamente a otro
            FilmingCalendar currentCalendar = calendar; countA += 1;
            FilmingCalendar newCalendar = new FilmingCalendar();
            for (int i = currentCalendar.Scenes.Count - 1; i >= 0; i--)
                newCalendar.Scenes.Add(currentCalendar.Scenes[i]);
            return newCalendar;
        }

        public static Scene sceneToChange(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar) {
            int count = 0; countA += 1;
            foreach (Scene scene in fatherCalendar.Scenes) {
                foreach (Scene auxScene in sonCalendar.Scenes)
                {
                    if (!scene.id.Equals(auxScene.id)) countC += 1;
                    {
                        count ++; countA += 1;
                        continue;
                    }
                    
                }
                if (count == sonCalendar.Scenes.Count) { countC += 1;
                    return scene;
                }
                    
                count = 0; countA += 1;
            }
            return null;
        }

        public static FilmingCalendar changeScenes(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar, int end)
        {
            List<string> idScenes = new List<string>();
            List<string> idScenes2 = new List<string>();
            FilmingCalendar newSonCalendar = new FilmingCalendar();
            for (int i = end;i< sonCalendar.Scenes.Count;i++) {
                idScenes2.Add(sonCalendar.Scenes[i].id);
            }
            for (int i=0; i< sonCalendar.Scenes.Count;i++) {
                Scene scene = sonCalendar.Scenes[i];  countA += 1;
                if (scene.marked == true && scene.id.Equals("0")) {  countC += 2;
                    foreach (Scene auxScene in fatherCalendar.Scenes)
                    {
                        bool exists = idScenes.Contains(auxScene.id); countA += 1;
                        bool exists2 = idScenes2.Contains(auxScene.id); countA += 1;
                        if (exists == false && exists2==false){ countC += 2;
                            newSonCalendar.Scenes.Add(auxScene);
                            idScenes.Add(auxScene.id);
                            break;
                        }
                    }
                }
                else {
                    idScenes.Add(scene.id);
                    newSonCalendar.Scenes.Add(scene);
                }   
            }
            return newSonCalendar;
        }

        public static FilmingCalendar createSonChromosome(int size, int start, int end, FilmingCalendar chromosome ) {
            FilmingCalendar sonCalendar = new FilmingCalendar(); countA += 1;
            foreach (Scene scene in chromosome.Scenes) {
                int index = chromosome.Scenes.IndexOf(scene); countA += 1;
                if (index > start && index < end && scene.marked == false) { countC += 3;
                    Scene newScene = new Scene(); countA += 1;
                    newScene.id = "0"; countA += 1;
                    newScene.marked = true; countA += 1;
                    sonCalendar.Scenes.Add(newScene);
                    continue;
                }
                else sonCalendar.Scenes.Add(scene);
            }
            return sonCalendar;
        }

        
        public static void performCrossingPMX(int positionScenario)
        {// Se realiza el cruce de las escenas  Recalcar este metodo crea dos descendientes a la vez
            
            int size = movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes.Count; countA += 1;
            int start = (size - size / 2) / 2; countA += 1;
            int end = (size - start) - 1; countA += 1;
            //Console.WriteLine("cantidad de elementos "+chromosome1.Scenes.Count);
            Console.WriteLine("start "+ start);
            Console.WriteLine("end " + end );
            FilmingCalendar descendent1;
            FilmingCalendar descendent2;

            for (int i = 0; i <2; i++)
            { // El cruce se realizará la cantidad de veces que se ejecute este for 
                FilmingCalendar chromosome1 = movie.Scenarios[positionScenario].FilmingCalendars[0]; countA += 1;
                FilmingCalendar chromosome2 = generateChromosome(chromosome1); countA += 1;

                descendent1 = createSonChromosome(size,start,end,chromosome1); countA += 1;
                descendent2 = createSonChromosome(size, start, end, chromosome2); countA += 1;

                FilmingCalendar newDesendent1 = changeScenes(chromosome2, descendent1,end); countA += 1;
                FilmingCalendar newDesendent2 = changeScenes(chromosome1, descendent2,end); countA += 1;
                accommodateScenesInDays(newDesendent1, positionScenario);
                accommodateScenesInDays(newDesendent2, positionScenario);

                /*for (int j=0;j<newDesendent1.Scenes.Count;j++) {
                    Console.WriteLine(chromosome1.Scenes[j].id+"........"+ chromosome2.Scenes[j].id);
                }
                Console.WriteLine("-------------------------------------------------------------");
                */
                chromosome1 = newDesendent1; countA += 1;
                chromosome2 = newDesendent2; countA += 1;
            }
        }

        public static List<Day> chooseTheBestCalendar(int positionScenario)
        {
            List<Day> bestList = movie.Scenarios[positionScenario].Days; countA += 1;
            int totalcost = Data.calculatePriceOfCalendar(positionScenario, movie.Scenarios[positionScenario].Days); countA += 1;
            //int count = -1;
            //Console.WriteLine("Numero de lista de dias "+movie.Scenarios[positionScenario].possibleDays.Count);
            foreach (List<Day> days in movie.Scenarios[positionScenario].possibleDays)
            {
                int cost = Data.calculatePriceOfCalendar(positionScenario, days); countA += 1;
                //count += 1;
                //Console.WriteLine(" " + count);
                if (cost < totalcost){ countC += 1;            
                    //Console.WriteLine("mejor " + totalcost);
                    bestList = days; countA += 1;
                    totalcost = cost; countA += 1;
                }
            }
            return bestList;
        }

        public static int availableSpace(Time Shedule)
        {// retorna cuanto espacio tiene la jornada segun sus paginas 
            int space = 0; countA += 1;
            foreach (Scene scene in Shedule.Scenes)
            {
                space += scene.Pages; countA += 1;
            }
            return Shedule.MaximunScriptPages - space;// Se le resta el espacio tatal al que ya posee, para obtener el que esta disponible 
        }

        public static bool canAssignedToActor(Day currentDay, List<Day> newDays, Scene scene)
        {// verifica el respeto de las horas a lavorar de cada actor 
            Day before;
            if (newDays.IndexOf(currentDay) != 0){ countC += 1;  //contexto de jornada de dia 
                before = newDays.ElementAt(newDays.IndexOf(currentDay) - 1); countA += 1; // el dia anterior 
                if (scene.Schedule == true){ countC += 1; // si la escena se realiza en el dia 
                    foreach (Scene auxScene in before.NightTime.Scenes)
                    {
                        foreach (Actor actor in auxScene.Actors)// recorre los actores del dia anterior
                        {
                            foreach (Actor auxActor in scene.Actors)// recorre los actores de la escena actual, la que se desea agregar
                            {
                                if (actor.ID.Equals(auxActor.ID)) countC += 1;
                                    return false;
                            }
                        }
                    }
                }
                if (scene.Schedule == false){ countC += 1; // si la escena se realiza en la noche 
                    foreach (Scene auxScene in before.DayTime.Scenes)
                    {
                        foreach (Actor actor in auxScene.Actors)
                        {// recorre los actores del dia anterior 
                            foreach (Actor auxActor in scene.Actors)
                            {// recorre los actores de la escena actual, la que se desea agregar
                                countC += 1;
                                if (actor.ID.Equals(auxActor.ID))
                                {  
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true; // si no se encontraron coincidencias o si es el primer dia 
        }

        public static List<Day> createListWithDays(FilmingCalendar calendar)
        {
            List<Day> newDays = new List<Day>();
            for (int i = 1; i <= calendar.Scenes.Count+4; i++)// se crean los dias para asignarle las escenas que ya fueron cruzdas  
            {
                Day newDay = new Day();
                newDay.DayNumber = i; countA += 1;
                Time time1 = new Time();
                time1.MaximunScriptPages = 45; countA += 1;
                Time time2 = new Time();
                time2.MaximunScriptPages = 45; countA += 1;
                newDay.DayTime = time1; countA += 1;
                newDay.NightTime = time2; countA += 1;
                newDays.Add(newDay); 
            }
            return newDays;
        }

        public static void accommodateScenesInDays(FilmingCalendar calendar, int positionScenario)
        {// resive el calendario que fue cruzado y se asignan las escenas del mismo a nuevos dias 
            List<Day> newDays = createListWithDays(calendar);

            foreach (Day day in newDays)
            {
                foreach (Scene scene in calendar.Scenes)
                {
                    countC += 1;
                    if (scene.Schedule == true)
                    {
                        int space = availableSpace(day.DayTime); countA += 1;   // obtiene espacio disponible 
                        countC += 1;
                        if (scene.Pages <= space)
                        {
                            bool can1 = canAssignedToActor(day, newDays, scene);// validar si los actores no estan el dia anterior en la jornada contraria 
                            countC += 1;
                            if (can1 == true)
                                day.DayTime.Scenes.Add(scene);   
                        }
                    }
                    countC += 1;
                    if (scene.Schedule == false)
                    {
                        int space = availableSpace(day.NightTime); countA += 1; // obtiene espacio disponible 
                        countC += 1;
                        if (scene.Pages <= space)
                        {
                            bool can2 = canAssignedToActor(day, newDays, scene);// validar si los actores no estan el dia anterior en la jornada contraria 
                            countC += 1;
                            if (can2 == true)
                                day.NightTime.Scenes.Add(scene);
                        }
                    }
                }
            }
            movie.Scenarios[positionScenario].possibleDays.Add(newDays);
        }
        /*------------------------------------------------------------------------------------------------------------------------------
                                                  ALGORITMO GENETICO OX
        --------------------------------------------------------------------------------------------------------------------------------
        */

        public static void clearLists() {// limpia la lista de posibles dias en cada escenario
            foreach (Scenario scenario in movie.Scenarios) {
                scenario.possibleDays.Clear();
            }
        }

        public static void performCrossingOX(int positionScenario)
        {// Se realiza el cruce de las escenas  Recalcar este metodo crea dos descendientes a la vez

            int size = movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes.Count; countA += 1;
            int start = (size - size / 2) / 2; countA += 1;
            int end = (size - start) - 1; countA += 1;
            //Console.WriteLine("cantidad de elementos "+chromosome1.Scenes.Count);
            Console.WriteLine("start " + start);
            Console.WriteLine("end " + end);
            FilmingCalendar descendent1;
            FilmingCalendar descendent2;

            for (int i = 0; i < 2; i++)
            { // El cruce se realizará la cantidad de veces que se ejecute este for 
                FilmingCalendar chromosome1 = movie.Scenarios[positionScenario].FilmingCalendars[0]; countA += 1;
                FilmingCalendar chromosome2 = generateChromosome(chromosome1); countA += 1;

                descendent1 = createSonChromosome(size, start, end, chromosome1); countA += 1;
                descendent2 = createSonChromosome(size, start, end, chromosome2); countA += 1;

                //FilmingCalendar newDesendent1 = changeScenes(chromosome2, descendent1, end); countA += 1;
                //FilmingCalendar newDesendent2 = changeScenes(chromosome1, descendent2, end); countA += 1;
                //accommodateScenesInDays(newDesendent1, positionScenario);
                //accommodateScenesInDays(newDesendent2, positionScenario);

                
                //chromosome1 = newDesendent1; countA += 1;
                //chromosome2 = newDesendent2; countA += 1;
            }
        }

        public static void performOxInAllScenarios()
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine("                            Algoritmo genetico OX"                                        );
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            for (int i = 0; i < 4; i++)
            {
                performCrossingOX(i);// posicion del escenario
                /*int numberOfScenario = i + 1;
                List<Day> days = chooseTheBestCalendar(i);// posición del escenario    
                int costo1 = Data.calculatePriceOfCalendar(i, movie.Scenarios[i].Days);
                int costo2 = Data.calculatePriceOfCalendar(i, days);
                Console.WriteLine("                         Escenario numero " + numberOfScenario);
                Console.WriteLine("calendario original el costo es de : " + costo1 + " calendario mutado el costo es de : " + costo2);
                */
            }

        }



    }
}


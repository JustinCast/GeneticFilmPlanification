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
        public static int countC, countA, countL = 0;// contador de comparaciones, asignaciones y lineas ejecutdas
        static Movie movie = Movie.GetInstance();
        /*------------------------------------------------------------------------------------------------------------------------------
                                                  ALGORITMO GENETICO PMX
        --------------------------------------------------------------------------------------------------------------------------------
        */
        public static FilmingCalendar generateChromosome(FilmingCalendar calendar)
        {// Se recorre las escenas de un calendario y se le van agregando inversamente a otro
            FilmingCalendar currentCalendar = calendar;
            FilmingCalendar newCalendar = new FilmingCalendar();
            countA += 3; countL += 3;
            for (int i = currentCalendar.Scenes.Count - 1; i >= 0; i--) {
                countA += 1; countC += 2;
                newCalendar.Scenes.Add(currentCalendar.Scenes[i]); countA += 1; countL += 2;
            }
            countC += 2; countL += 2;
            return newCalendar;
        }

        public static Scene sceneToChange(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar) {
            int count = 0;
            countA += 2; countL += 2;
            foreach (Scene scene in fatherCalendar.Scenes) {
                countL += 1; countA += 2;
                foreach (Scene auxScene in sonCalendar.Scenes)
                {
                    countC += 1; countL += 2; countA += 1;
                    if (!scene.id.Equals(auxScene.id))
                    {
                        count++; countA += 1; countL += 2;
                        continue;
                    }
                }
                countC += 1; countL += 2;
                if (count == sonCalendar.Scenes.Count) {
                    countL += 1;
                    return scene;
                }
                count = 0; countA += 1; countL += 1;
            }
            countL += 2;
            return null;
        }

        public static FilmingCalendar makeMutation(FilmingCalendar calendar) {
            FilmingCalendar currenCalendar=calendar;
            currenCalendar.Scenes[4] = calendar.Scenes[6];
            currenCalendar.Scenes[6] = calendar.Scenes[4];
            countL += 5; countA += 3;
            return currenCalendar;
        }

        public static FilmingCalendar changeScenes(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar, int end)
        {
            List<string> idScenes = new List<string>();
            List<string> idScenes2 = new List<string>();
            FilmingCalendar newSonCalendar = new FilmingCalendar();
            countA += 4; countL += 4;
            for (int i = end; i < sonCalendar.Scenes.Count; i++)
                idScenes2.Add(sonCalendar.Scenes[i].id); countA += 2; countL += 2; countC += 1;
            countA += 1; countC += 1; countL += 1;
            for (int i = 0; i < sonCalendar.Scenes.Count; i++) {
                Scene scene = sonCalendar.Scenes[i]; countA += 2; countL += 2;
                countC += 4; countL += 1;
                if (scene.marked == true && scene.id.Equals("0")) {
                    countA += 1;
                    foreach (Scene auxScene in fatherCalendar.Scenes)
                    {
                        countL += 1; countA += 1;
                        bool exists = idScenes.Contains(auxScene.id); countA += 1; countL += 1;
                        bool exists2 = idScenes2.Contains(auxScene.id); countA += 1; countL += 1;
                        countC += 3; countL += 1;
                        if (exists == false && exists2 == false) {
                            newSonCalendar.Scenes.Add(auxScene); countA += 1; countL += 1;
                            idScenes.Add(auxScene.id); countA += 1; countL += 2;
                            break;
                        }
                    }
                    countL += 1;
                }
                else {
                    idScenes.Add(scene.id); countA += 1; countL += 1;
                    newSonCalendar.Scenes.Add(scene); countA += 1; countL += 1;
                }
            }
            countL += 2; countC += 1;
            return makeMutation(newSonCalendar);
        }

        public static FilmingCalendar createSonChromosome(int size, int start, int end, FilmingCalendar chromosome) {
            FilmingCalendar sonCalendar = new FilmingCalendar(); countA += 2; countL += 2;
            foreach (Scene scene in chromosome.Scenes) {
                int index = chromosome.Scenes.IndexOf(scene); countA += 2; countL += 2;
                countC += 5; countL += 1;
                if (index > start && index < end && scene.marked == false) {
                    Scene newScene = new Scene();
                    newScene.id = "0";
                    newScene.marked = true;
                    countA += 3; countL += 3;
                    sonCalendar.Scenes.Add(newScene); countA += 1; countL += 2;
                    continue;
                }
                else sonCalendar.Scenes.Add(scene); countA += 1; countL += 1;
            }
            countL += 2;
            return sonCalendar;
        }


        public static void performCrossingPMX(int positionScenario)
        {// Se realiza el cruce de las escenas  Recalcar este metodo crea dos descendientes a la vez
            int size = movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes.Count;
            int start = (size - size / 2) / 2;
            int end = (size - start) - 1;
            FilmingCalendar descendent1;
            FilmingCalendar chromosome1 = null;
            FilmingCalendar chromosome2 = null;
            countA += 6; countL += 7;
            for (int i = 0; i < 4; i++)
            { // El cruce se realizará la cantidad de veces que se ejecute este for 
                countA += 1; countC += 2;
                if (i==0) {
                    chromosome1 = movie.Scenarios[positionScenario].FilmingCalendars[0];
                    chromosome2 = generateChromosome(chromosome1);
                    countA += 2;
                }              
                descendent1 = createSonChromosome(size, start, end, chromosome1);
                FilmingCalendar newDesendent1 = changeScenes(chromosome2, descendent1, end);
                clearAssigned(newDesendent1);
                List<Day> dayCalendar1 = accommodateScenesInDays(newDesendent1);
                movie.Scenarios[positionScenario].possibleDays.Add(dayCalendar1);
                chromosome1 = chromosome2;
                chromosome2 = generateChromosome(newDesendent1);
                countA += 5; countL += 11;
            }
            countL += 1; countC += 1;
        }

        public static List<Day> chooseTheBestCalendar(int positionScenario)
        {
            List<Day> bestList = movie.Scenarios[positionScenario].Days;
            int totalcost = Data.calculatePriceOfCalendar(positionScenario, movie.Scenarios[positionScenario].Days);
            countA += 3; countL += 3;
            foreach (List<Day> days in movie.Scenarios[positionScenario].possibleDays)
            {
                countA += 1;
                int cost = Data.calculatePriceOfCalendar(positionScenario, days); countA += 1; countL += 2;
                countC += 1; countL += 1;
                if (cost < totalcost) {
                    bestList = days;
                    totalcost = cost;
                    countA += 2; countL += 2;
                }
            }
            countL += 2;countC += 1;
            return bestList;
        }

        public static int availableSpace(Time Shedule)
        {// retorna cuanto espacio tiene la jornada segun sus paginas 
            int space = 0; countA += 2; countL += 2;
            foreach (Scene scene in Shedule.Scenes)
                space += scene.Pages; countA += 2; countL += 2;
            countL += 2;
            return Shedule.MaximunScriptPages - space;// Se le resta el espacio tatal al que ya posee, para obtener el que esta disponible 
        }

        public static bool canAssignedScene(Day currentDay, List<Day> newDays, Scene scene) {
            countL += 2; countC += 1;
            if (newDays.IndexOf(currentDay)==0) {
                foreach (Actor actor in scene.Actors) {
                    countA += 1; countC += 1;countL += 2;
                    if (scene.Schedule==true) {
                        foreach (Scene scene1 in currentDay.NightTime.Scenes) {
                            countA += 1;countC += 1;countL += 2;
                            if (scene.Actors.Contains(actor) == true) {
                                countL += 1;
                                return false;
                            }
                            countL += 1;
                            continue;
                        }
                        countA += 1; countL += 1;
                    }
                    countC += 1;
                    if (scene.Schedule == false)
                    {
                        foreach (Scene scene1 in currentDay.DayTime.Scenes)
                        {
                            countA += 1;countC += 1; countL += 2;
                            if (scene.Actors.Contains(actor) == true) {
                                countL += 1;
                                return false;
                            }                       
                            continue;
                        }
                        countA += 1; countL += 1;
                    }
                }
                countA += 1;countL += 2;
                return true;  
            }
            int index = newDays.IndexOf(currentDay) - 1;
            Day before = newDays[index];
            countA += 2; countL += 2;
            foreach (Actor actor in scene.Actors) {
                countA += 1; countC += 1;countL += 2;
                if (scene.Schedule==true)
                {
                    foreach (Scene scene1 in before.NightTime.Scenes) {
                        countA += 1; countL += 2;countC += 1;
                        if (scene.Actors.Contains(actor) == true) {
                            countL += 1;
                            return false;
                        }
                    }
                    countA += 1; countL += 1;

                    foreach (Scene scene2 in currentDay.NightTime.Scenes) {
                        countA += 1; countL += 2;countC += 1;
                        if (scene2.Actors.Contains(actor) == true) {
                            countL +=1;
                            return false;
                        }   
                    }
                    countA += 1; countL += 2;
                    return true;
                }
                countC += 1; countL += 1;
                if (scene.Schedule==false)
                {
                    foreach (Scene scene1 in before.DayTime.Scenes)
                    {
                        countA += 1; countL += 2;countC += 1;
                        if (scene.Actors.Contains(actor) == true) {
                            countL += 1;
                            return false;
                        }
                            
                    }
                    countA += 1; countL += 1;
                    foreach (Scene scene2 in currentDay.DayTime.Scenes)
                    {
                        countA += 1; countL += 2; countC += 1;
                        if (scene2.Actors.Contains(actor) == true) {
                            countL += 1;
                            return false;
                        }        
                    }
                    countA += 1; countL += 2;
                    return true;
                }
            }
            countA += 1; countL += 2;
            return true;
        }

        public static List<Day> createListWithDays(FilmingCalendar calendar)
        {
            List<Day> newDays = new List<Day>(); countA += 2; countL += 2;
            for (int i = 1; i <= calendar.Scenes.Count-5; i++)// se crean los dias para asignarle las escenas que ya fueron cruzdas  
            {
                countL += 1; countA += 1; countC += 2;
                Day newDay = new Day();        
                newDay.DayNumber = i;          
                Time time1 = new Time();       
                time1.MaximunScriptPages = 45; 
                Time time2 = new Time();       
                time2.MaximunScriptPages = 45; 
                newDay.DayTime = time1;        
                newDay.NightTime = time2;      
                newDays.Add(newDay);
                countA += 9; countL += 9;
            }
            countL += 2; countC += 1;
            return newDays;
        }

        public static List<Day> accommodateScenesInDays(FilmingCalendar calendar)
        {// resive el calendario que fue cruzado y se asignan las escenas del mismo a nuevos dias 
            List<Day> newDays = createListWithDays(calendar); countA += 1; countL += 2;
            foreach (Day day in newDays)
            { 
                countA += 1; countL += 1;
                foreach (Scene scene in calendar.Scenes)
                {
                    countA += 1; countL += 2; countC += 1;
                    if (scene.assigned==false) {
                        countC += 1; countL += 1;
                        if (scene.Schedule == true)
                        {
                            int space = availableSpace(day.DayTime); countA += 1; countL += 2;  // obtiene espacio disponible 
                            countC += 2;
                            if (scene.Pages <= space)
                            {
                                bool can1 = canAssignedScene(day, newDays, scene); countA += 1; countL += 1; // validar si los actores no estan el dia anterior en la jornada contraria 
                                countC += 1; countL += 1;
                                if (can1 == true) {
                                    scene.assigned = true;
                                    day.DayTime.Scenes.Add(scene); countA += 2; countL += 2;
                                }  
                            }
                        }
                        countC += 1; countL += 1;
                        if (scene.Schedule == false)
                        {
                            int space = availableSpace(day.NightTime); countA += 1;// obtiene espacio disponible 
                            countC += 2; countL += 2;
                            if (scene.Pages <= space)
                            {
                                bool can2 = canAssignedScene(day, newDays, scene); countA += 1;// validar si los actores no estan el dia anterior en la jornada contraria 
                                countC += 1; countL += 2;
                                if (can2 == true) {
                                    scene.assigned = true;
                                    day.NightTime.Scenes.Add(scene); countA += 2; countL += 2;
                                }    
                            }
                        }
                    }   
                }
                countA += 1; countL += 1;
            }
            countA += 1; countL += 1;
            return newDays;
        }
        /*------------------------------------------------------------------------------------------------------------------------------
                                                  ALGORITMO GENETICO OX
        --------------------------------------------------------------------------------------------------------------------------------
        */

        public static void clearLists() {// limpia la lista de posibles dias en cada escenario
            countL += 1;
            foreach (Scenario scenario in movie.Scenarios)
            {
                scenario.possibleDays.Clear(); countL += 2; countA += 2;
            }
            countL += 1;
        }

        public static FilmingCalendar createSonChromosomeOX(int size, int start1, int end1, FilmingCalendar chromosome)
        {
            FilmingCalendar sonCalendar = new FilmingCalendar(); countA += 1; countL += 2;
            foreach (Scene scene in chromosome.Scenes)
            {
                int index = chromosome.Scenes.IndexOf(scene); countA += 2; countL += 2;
                countC += 1; countL += 1;
                if (scene.marked == false) {
                    countC += 5; countL += 1;
                    if (index >= start1 && index <= end1)
                    {
                        Scene newScene = new Scene();
                        newScene.id = "0";
                        newScene.marked = true; 
                        sonCalendar.Scenes.Add(newScene); 
                        countA += 4; countL += 5;
                        continue;
                    }   
                    else sonCalendar.Scenes.Add(scene); countA += 1; countL += 1;
                }   
            }
            countL += 2;
            return sonCalendar;
        }

        public static FilmingCalendar changeScenesOX(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar)
        {
            List<string> idScenes = new List<string>();             
            List<string> idScenes2 = new List<string>();            
            FilmingCalendar newSonCalendar = new FilmingCalendar(); 
            countA += 4; countL += 4;
            for (int i = 0;i< sonCalendar.Scenes.Count;i++) {
                countC += 2; countL += 2; countA += 1;
                if (sonCalendar.Scenes[i].id!="0") 
                    idScenes2.Add(sonCalendar.Scenes[i].id); countL += 1; countA += 1;
            }
            countC += 1; countA += 1; countL += 1;
            for (int i=0; i< sonCalendar.Scenes.Count;i++) {
                countA += 1; countC += 1;countL += 1;
                Scene scene = sonCalendar.Scenes[i];  countA += 1; countL += 1;
                countC += 3; countL += 1;
                if (scene.marked == true && scene.id.Equals("0")) {
                    foreach (Scene auxScene in fatherCalendar.Scenes)
                    {
                        countL += 1; countA += 1;
                        bool exists = idScenes.Contains(auxScene.id); countA += 1; countL += 1;
                        bool exists2 = idScenes2.Contains(auxScene.id); countA += 1; countL += 1;
                        countC += 3; countL += 1;
                        if (exists == false && exists2==false){ 
                            newSonCalendar.Scenes.Add(auxScene); countA += 1; countL += 1;
                            idScenes.Add(auxScene.id); countA += 1; countL += 2;
                            break;
                        }
                    }
                    countL += 1;
                }
                else {
                    idScenes.Add(scene.id); countL += 1; countA += 1;
                    newSonCalendar.Scenes.Add(scene); countA += 1; countL += 1;
                }   
            }
            countL += 2;countC += 1;
            return makeMutation(newSonCalendar);
        }

        public static void performCrossingOX(int positionScenario)
        {// Se realiza el cruce de las escenas  Recalcar este metodo crea dos descendientes a la vez
            int size = movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes.Count;
            int start1 = 1; countA += 1; 
            int end1= ((size - size / 2) / 2)+2;  
            FilmingCalendar descendent1;
            FilmingCalendar chromosome1 = null;
            FilmingCalendar chromosome2 = null;
            countA += 6; countL += 7;
            for (int i = 0; i < 4; i++)
            { // El cruce se realizará la cantidad de veces que se ejecute este for 
                countL += 1; countC += 2; countA += 1;
                if (i == 0)
                {
                    chromosome1 = movie.Scenarios[positionScenario].FilmingCalendars[0];
                    chromosome2 = generateChromosome(chromosome1);
                    countA += 2; countL += 2;
                }
                descendent1 = createSonChromosomeOX(size, start1, end1, chromosome1);
                FilmingCalendar newDesendent1 = changeScenesOX(chromosome2, descendent1);
                clearAssigned(newDesendent1);
                List<Day> dayCalendar1=accommodateScenesInDays(newDesendent1);
                movie.Scenarios[positionScenario].possibleDays.Add(dayCalendar1);
                chromosome1 = chromosome2;
                chromosome2 = generateChromosome(newDesendent1);
                countA += 6; countL += 7;
            }
            countL += 1;countC += 1;
        }

        public static void performOxInAllScenarios()
        {
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("                                                                        Algoritmo genetico OX"                                        );
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            for (int i = 0; i < 4; i++)
            {
                performCrossingOX(i);// posicion del escenario
                int numberOfScenario = i + 1;
                List<Day> days = chooseTheBestCalendar(i);// posición del escenario 
                int costo1 = Data.calculatePriceOfCalendar(i, movie.Scenarios[i].Days);
                int costo2 = Data.calculatePriceOfCalendar(i, days);
                Console.WriteLine("                                                                      Escenario numero " + numberOfScenario);
                Console.WriteLine("                                                            Calendario original: " + costo1 + " Calendario mutado: " + costo2);
                Console.WriteLine("                                                          \n" +
                                  "                                                      Asignaciones: " + Pmx.countA + "     Comparaciones: " + Pmx.countC + "     Lineas ejecutadas: " + Pmx.countL + "\n");
                Pmx.countA = 0; Pmx.countL = 0; Pmx.countC = 0;
                /*Console.WriteLine("    CALENDARIO INICIAL");
                Pmx.printDays(movie.Scenarios[i].Days);
                Console.WriteLine("\n    CALENDARIO CRUZADO");
                Pmx.printDays(days);*/
            }
        }

        public static void printDays(List<Day> currentDays) {// impresion de los dias con las escenas normales y con las escenas ya cruzadas     
            Console.WriteLine("------------------------------------------------------------");
            List<string> actores = new List<string>();
            foreach (Day day in currentDays) {
                if (day.NightTime.Scenes.Count == 0 && day.DayTime.Scenes.Count == 0)
                {
                    continue;
                }
                Console.WriteLine("                    Dia " + day.DayNumber);
                foreach (Scene scene in day.DayTime.Scenes)
                {
                    Console.WriteLine("  Escena " + scene.id + " jornada de dia");
                    foreach (Actor actor in scene.Actors)
                    {
                        actores.Add(actor.ID);
                    }
                    if (actores.Count==3) {
                        Console.WriteLine("  Actores: "+ "ID: "+actores[0] + "   " + "ID: " + actores[1] + "   " + "ID: " + actores[2]);
                    }
                    if (actores.Count == 4)
                    {
                        Console.WriteLine("  Actores: " + "ID: " + actores[0] + "   " + "ID: " + actores[1] + "   " + "ID: " + actores[2] + "   " + "ID: " + actores[3]);

                    }
                    if (actores.Count == 5)
                    {
                        Console.WriteLine("  Actores: " + "ID: " + actores[0] + "   " + "ID: " + actores[1] + "   " + "ID: " + actores[2] + "   " + "ID: " + actores[3] + "   " + "ID: " + actores[4]);
                    }
                    
                    actores.Clear();
                }
                foreach (Scene scene in day.NightTime.Scenes)
                {
                    Console.WriteLine("  Escena " + scene.id + " jornada de noche");
                    foreach (Actor actor in scene.Actors)
                    {
                        actores.Add(actor.ID);
                    }
                    if (actores.Count == 3)
                    {
                        Console.WriteLine("  Actores: " + "ID: " + actores[0] + "   " + "ID: " + actores[1] + "   " + "ID: " + actores[2]);
                    }
                    if (actores.Count == 4)
                    {
                        Console.WriteLine("  Actores: " + "ID: " + actores[0] + "   " + "ID: " + actores[1] + "   " + "ID: " + actores[2] + "   " + "ID: " + actores[3]);

                    }
                    if (actores.Count == 5)
                    {
                        Console.WriteLine("  Actores: " + "ID: " + actores[0] + "   " + "ID: " + actores[1] + "   " + "ID: " + actores[2] + "   " + "ID: " + actores[3] + "   " + "ID: " + actores[4]);
                    }
                    actores.Clear();
                }
            }           
        }

        
        public static void imprimir() {
            foreach (Scenario scenario in movie.Scenarios) {
                Console.WriteLine("-----------------------------------------------");
                foreach (Scene scene in scenario.FilmingCalendars[0].Scenes) {
                    Console.WriteLine("Scene " +scene.id+"  --------------------------------"+ scene.assigned);
                    foreach (Actor actor in scene.Actors) {
                        Console.WriteLine("Actor "+actor.ID);
                    }
                }
            }
        }

        public static void clearAssigned(FilmingCalendar calendar) {
            countL += 1;
            foreach (Scene scene in calendar.Scenes) { 
                scene.assigned = false;
                countA += 2;countL += 2;
            }
            countL += 1;
        }
    }
}


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
        static int countC, countA,countL = 0;// contador de comparaciones, asignaciones y lineas ejecutdas
        static Movie movie = Movie.GetInstance();
        /*------------------------------------------------------------------------------------------------------------------------------
                                                  ALGORITMO GENETICO PMX
        --------------------------------------------------------------------------------------------------------------------------------
        */
        public static FilmingCalendar generateChromosome(FilmingCalendar calendar)
        {// Se recorre las escenas de un calendario y se le van agregando inversamente a otro
            FilmingCalendar currentCalendar = calendar; countA += 1; countL += 1;
            FilmingCalendar newCalendar = new FilmingCalendar(); countA += 1; countL += 1;
            for (int i = currentCalendar.Scenes.Count - 1; i >= 0; i--) {
                newCalendar.Scenes.Add(currentCalendar.Scenes[i]); countL += 2;
            }
            countL += 1;
            return newCalendar; 
        }

        public static Scene sceneToChange(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar) {
            int count = 0; countA += 1; countL += 1;
            foreach (Scene scene in fatherCalendar.Scenes) {
                countL += 1;
                foreach (Scene auxScene in sonCalendar.Scenes)
                {
                    countC += 1; countL += 2;
                    if (!scene.id.Equals(auxScene.id)) 
                    {
                        count ++; countA += 1; countL += 2;
                        continue;
                    }   
                }
                countC += 1; countL += 1;
                if (count == sonCalendar.Scenes.Count) {
                    countL += 1;
                    return scene;
                }
                count = 0; countA += 1; countL += 1;
            }
            countL += 1;
            return null;
        }

        public static FilmingCalendar changeScenes(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar, int end)
        {
            List<string> idScenes = new List<string>(); 
            List<string> idScenes2 = new List<string>(); 
            FilmingCalendar newSonCalendar = new FilmingCalendar();
            countA += 3; countL += 3;
            for (int i = end;i< sonCalendar.Scenes.Count;i++) 
                idScenes2.Add(sonCalendar.Scenes[i].id); countL += 2;
            for (int i=0; i< sonCalendar.Scenes.Count;i++) {
                Scene scene = sonCalendar.Scenes[i];  countA += 1; countL += 2;
                countC += 3; countL += 1;
                if (scene.marked == true && scene.id.Equals("0")) {  
                    foreach (Scene auxScene in fatherCalendar.Scenes)
                    {
                        countL += 1;
                        bool exists = idScenes.Contains(auxScene.id); countA += 1; countL += 1;
                        bool exists2 = idScenes2.Contains(auxScene.id); countA += 1; countL += 1;
                        countC += 3; countL += 1;
                        if (exists == false && exists2==false){ 
                            newSonCalendar.Scenes.Add(auxScene); countL += 1;
                            idScenes.Add(auxScene.id); countL += 2;
                            break;
                        }
                    }
                }
                else {
                    idScenes.Add(scene.id); countL += 1;
                    newSonCalendar.Scenes.Add(scene); countL += 1;
                }   
            }
            countL += 1;
            return newSonCalendar;
        }

        public static FilmingCalendar createSonChromosome(int size, int start, int end, FilmingCalendar chromosome ) {
            FilmingCalendar sonCalendar = new FilmingCalendar(); countA += 1; countL += 1;
            foreach (Scene scene in chromosome.Scenes) {
                int index = chromosome.Scenes.IndexOf(scene); countA += 1; countL += 2;
                countC += 5; countL += 1;
                if (index > start && index < end && scene.marked == false) { 
                    Scene newScene = new Scene();
                    newScene.id = "0";
                    newScene.marked = true;
                    countA += 3; countL += 3;
                    sonCalendar.Scenes.Add(newScene); countL += 2;
                    continue;
                }
                else sonCalendar.Scenes.Add(scene); countL += 1;
            }
            countL += 1;
            return sonCalendar;
        }

        
        public static void performCrossingPMX(int positionScenario)
        {// Se realiza el cruce de las escenas  Recalcar este metodo crea dos descendientes a la vez
            int size = movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes.Count;
            int start = (size - size / 2) / 2; 
            int end = (size - start) - 1; 
            FilmingCalendar descendent1; 
            FilmingCalendar descendent2; 
            countA += 3; countL += 5;
            for (int i = 0; i <1; i++)
            { // El cruce se realizará la cantidad de veces que se ejecute este for 
                FilmingCalendar chromosome1 = movie.Scenarios[positionScenario].FilmingCalendars[0];
                FilmingCalendar chromosome2 = generateChromosome(chromosome1);
                descendent1 = createSonChromosome(size,start,end,chromosome1); 
                descendent2 = createSonChromosome(size, start, end, chromosome2); 
                FilmingCalendar newDesendent1 = changeScenes(chromosome2, descendent1,end); 
                FilmingCalendar newDesendent2 = changeScenes(chromosome1, descendent2,end); 
                accommodateScenesInDays(newDesendent1, positionScenario); 
                accommodateScenesInDays(newDesendent2, positionScenario); 
                chromosome1 = newDesendent1; 
                chromosome2 = newDesendent2;
                countA += 8; countL += 10;
            }
        }

        public static List<Day> chooseTheBestCalendar(int positionScenario)
        {
            List<Day> bestList = movie.Scenarios[positionScenario].Days; 
            int totalcost = Data.calculatePriceOfCalendar(positionScenario, movie.Scenarios[positionScenario].Days);
            countA += 2; countL += 2;
            foreach (List<Day> days in movie.Scenarios[positionScenario].possibleDays)
            {
                int cost = Data.calculatePriceOfCalendar(positionScenario, days); countA += 1; countL += 2;
                countC += 1; countL += 1;
                if (cost < totalcost){            
                    bestList = days; countA += 1; countL += 1;
                    totalcost = cost; countA += 1; countL += 1;
                }
            }
            countL += 1;
            return bestList;
        }

        public static int availableSpace(Time Shedule)
        {// retorna cuanto espacio tiene la jornada segun sus paginas 
            int space = 0; countA += 1; countL += 1;
            foreach (Scene scene in Shedule.Scenes)    
                space += scene.Pages; countA += 1; countL += 2;
            countL += 1;
            return Shedule.MaximunScriptPages - space;// Se le resta el espacio tatal al que ya posee, para obtener el que esta disponible 
        }

        public static bool canAssignedToActor(Day currentDay, List<Day> newDays, Scene scene)
        {// verifica el respeto de las horas a lavorar de cada actor 
            Day before; countL += 1;
            countC += 1;
            if (newDays.IndexOf(currentDay) != 0){  //contexto de jornada de dia 
                before = newDays.ElementAt(newDays.IndexOf(currentDay) - 1); countA += 1; countL += 2; // el dia anterior 
                countC += 1; countL += 1;
                if (scene.Schedule == true){  // si la escena se realiza en el dia 
                    foreach (Scene auxScene in before.NightTime.Scenes)
                    {
                        countL += 1;
                        foreach (Actor actor in auxScene.Actors)// recorre los actores del dia anterior
                        {
                            countL += 1;
                            foreach (Actor auxActor in scene.Actors)// recorre los actores de la escena actual, la que se desea agregar
                            {
                                countC += 1; countL += 1;
                                if (actor.ID.Equals(auxActor.ID)) {
                                    countL += 1;
                                    continue;
                                }
                                countL += 1;
                                return false;
                            }
                        }
                    }
                }
                countC += 1; countL += 1;
                if (scene.Schedule == false){ // si la escena se realiza en la noche 
                    foreach (Scene auxScene in before.DayTime.Scenes)
                    {
                        countL += 1;
                        foreach (Actor actor in auxScene.Actors)
                        {// recorre los actores del dia anterior 
                            countL += 1;
                            foreach (Actor auxActor in scene.Actors)
                            {// recorre los actores de la escena actual, la que se desea agregar
                                countC += 1; countL += 2;
                                if (actor.ID.Equals(auxActor.ID)) {
                                    countL += 1;
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            countL += 1;
            return true; // si no se encontraron coincidencias o si es el primer dia 
        }

        public static List<Day> createListWithDays(FilmingCalendar calendar)
        {
            List<Day> newDays = new List<Day>(); countA += 1; countL += 1;
            for (int i = 1; i <= calendar.Scenes.Count+4; i++)// se crean los dias para asignarle las escenas que ya fueron cruzdas  
            {
                countL += 1;
                Day newDay = new Day();        
                newDay.DayNumber = i;          
                Time time1 = new Time();       
                time1.MaximunScriptPages = 45; 
                Time time2 = new Time();       
                time2.MaximunScriptPages = 45; 
                newDay.DayTime = time1;        
                newDay.NightTime = time2;      
                newDays.Add(newDay);                        
                countA += 8; countL += 9;
            }
            countL += 1;
            return newDays;
        }

        public static void accommodateScenesInDays(FilmingCalendar calendar, int positionScenario)
        {// resive el calendario que fue cruzado y se asignan las escenas del mismo a nuevos dias 
            List<Day> newDays = createListWithDays(calendar); countA += 1; countL += 1;
            foreach (Day day in newDays)
            {
                countL += 1;
                foreach (Scene scene in calendar.Scenes)
                {
                    countC += 1; countL += 2;
                    if (scene.Schedule == true)
                    {
                        int space = availableSpace(day.DayTime); countA += 1; countL += 1;  // obtiene espacio disponible 
                        countC += 2; countL += 2;
                        if (scene.Pages <= space)
                        {
                            bool can1 = canAssignedToActor(day, newDays, scene); countA += 1; countL += 1; // validar si los actores no estan el dia anterior en la jornada contraria 
                            countC += 1; countL += 1;
                            if (can1 == true)
                                day.DayTime.Scenes.Add(scene); countL += 1;
                        }
                    }
                    countC += 1; countL += 1;
                    if (scene.Schedule == false)
                    {
                        int space = availableSpace(day.NightTime); countA += 1; countL += 1;// obtiene espacio disponible 
                        countC += 2; countL += 1;
                        if (scene.Pages <= space)
                        {
                            bool can2 = canAssignedToActor(day, newDays, scene); countA += 1; countL += 1;// validar si los actores no estan el dia anterior en la jornada contraria 
                            countC += 1; countL += 1;
                            if (can2 == true)
                                day.NightTime.Scenes.Add(scene); countL += 1;
                        }
                    }
                }
            }
            movie.Scenarios[positionScenario].possibleDays.Add(newDays); countL += 1;
        }
        /*------------------------------------------------------------------------------------------------------------------------------
                                                  ALGORITMO GENETICO OX
        --------------------------------------------------------------------------------------------------------------------------------
        */

        public static void clearLists() {// limpia la lista de posibles dias en cada escenario
            foreach (Scenario scenario in movie.Scenarios)
                scenario.possibleDays.Clear(); countL += 2;
        }

        public static FilmingCalendar createSonChromosomeOX(int size, int start1, int start2, int end1, int end2, FilmingCalendar chromosome)
        {
            FilmingCalendar sonCalendar = new FilmingCalendar(); countA += 1; countL += 1;
            foreach (Scene scene in chromosome.Scenes)
            {
                int index = chromosome.Scenes.IndexOf(scene); countA += 1; countL += 2;
                countC += 1; countL += 1;
                if (scene.marked == false) {
                    countC += 11; countL += 1;
                    if (index >= start1 && index <= end1 || index >= start2 && index <= end2)
                    {
                        Scene newScene = new Scene();
                        newScene.id = "0";
                        newScene.marked = true; 
                        sonCalendar.Scenes.Add(newScene); 
                        countA += 3; countL += 5;
                        continue;
                    }   
                    else sonCalendar.Scenes.Add(scene); countL += 1;
                }   
            }
            countL += 1;
            return sonCalendar;
        }

        public static FilmingCalendar changeScenesOX(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar)
        {
            List<string> idScenes = new List<string>();             
            List<string> idScenes2 = new List<string>();            
            FilmingCalendar newSonCalendar = new FilmingCalendar(); 
            countA += 3; countL += 3;
            for (int i = 0;i< sonCalendar.Scenes.Count;i++) {
                countC += 1; countL += 2;
                if (sonCalendar.Scenes[i].id!="0") 
                    idScenes2.Add(sonCalendar.Scenes[i].id); countL += 1;
            }
            for (int i=0; i< sonCalendar.Scenes.Count;i++) {
                Scene scene = sonCalendar.Scenes[i];  countA += 1; countL += 2;
                countC += 3; countL += 1;
                if (scene.marked == true && scene.id.Equals("0")) { 
                    foreach (Scene auxScene in fatherCalendar.Scenes)
                    {
                        countL += 1;
                        bool exists = idScenes.Contains(auxScene.id); countA += 1; countL += 1;
                        bool exists2 = idScenes2.Contains(auxScene.id); countA += 1; countL += 1;
                        countC += 3; countL += 1;
                        if (exists == false && exists2==false){ 
                            newSonCalendar.Scenes.Add(auxScene); countL += 1;
                            idScenes.Add(auxScene.id); countL += 2;
                            break;
                        }
                    }
                }
                else {
                    idScenes.Add(scene.id); countL += 1;
                    newSonCalendar.Scenes.Add(scene); countL += 1;
                }   
            }
            countL += 1;
            return newSonCalendar;
        }

        public static void performCrossingOX(int positionScenario)
        {// Se realiza el cruce de las escenas  Recalcar este metodo crea dos descendientes a la vez
            int size = movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes.Count; 
            int start1 = 1; countA += 1; 
            int end1= ((size - size / 2) / 2) + 1; 
            int start2= (size - start1)-3; 
            int end2 = (size - 1)-1; 
            FilmingCalendar descendent1; 
            FilmingCalendar descendent2; 
            countA += 5; countL += 7;
            for (int i = 0; i < 1; i++)
            { // El cruce se realizará la cantidad de veces que se ejecute este for 
                FilmingCalendar chromosome1 = movie.Scenarios[positionScenario].FilmingCalendars[0]; 
                FilmingCalendar chromosome2 = generateChromosome(chromosome1); 
                descendent1 = createSonChromosomeOX(size, start1, start2, end1, end2, chromosome1); 
                descendent2 = createSonChromosomeOX(size, start1, start2, end1, end2, chromosome1); 
                FilmingCalendar newDesendent1 = changeScenesOX(chromosome2, descendent1); 
                FilmingCalendar newDesendent2 = changeScenesOX(chromosome1, descendent2); 
                accommodateScenesInDays(newDesendent1, positionScenario); 
                accommodateScenesInDays(newDesendent2, positionScenario); 
                chromosome1 = newDesendent1; 
                chromosome2 = newDesendent2;
                countA += 8; countL += 10;
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
                int numberOfScenario = i + 1;
                List<Day> days = chooseTheBestCalendar(i);// posición del escenario    
                int costo1 = Data.calculatePriceOfCalendar(i, movie.Scenarios[i].Days);
                int costo2 = Data.calculatePriceOfCalendar(i, days);
                Console.WriteLine("                         Escenario numero " + numberOfScenario);
                Console.WriteLine("calendario original el costo es de : " + costo1 + " calendario mutado el costo es de : " + costo2);
            }
        }
    }
}


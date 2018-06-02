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
        static Movie movie = Movie.GetInstance();

        public static FilmingCalendar generateChromosome(FilmingCalendar calendar)
        {// Se recorre las escenas de un calendario y se le van agregando inversamente a otro
            FilmingCalendar currentCalendar = calendar;
            FilmingCalendar newCalendar = new FilmingCalendar();
            for (int i = currentCalendar.Scenes.Count - 1; i >= 0; i--)
            {
                newCalendar.Scenes.Add(currentCalendar.Scenes[i]);
            }
            return newCalendar;
        }

        public static Scene sceneToChange(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar) {
            int count = 0;
            foreach (Scene scene in fatherCalendar.Scenes) {
                foreach (Scene auxScene in sonCalendar.Scenes)
                {
                    if (!scene.id.Equals(auxScene.id))
                    {
                        count ++;
                        continue;
                    }
                    break;
                }
                if (count== sonCalendar.Scenes.Count) {

                    return scene;
                }
                count = 0;
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
                Scene scene = sonCalendar.Scenes[i];
                if (scene.marked == true && scene.id.Equals("0"))
                {
                    foreach (Scene auxScene in fatherCalendar.Scenes)
                    {
                        bool exists = idScenes.Contains(auxScene.id);
                        bool exists2 = idScenes2.Contains(auxScene.id);
                        if (exists == false && exists2==false)
                        {
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
            for (int r = 0; r < fatherCalendar.Scenes.Count; r++)
            {
                Console.WriteLine(fatherCalendar.Scenes[r].id + " ...." + newSonCalendar.Scenes[r].id);
            }
            return newSonCalendar;
        }

        public static FilmingCalendar createSonChromosome(int size, int start, int end, FilmingCalendar chromosome ) {
            FilmingCalendar sonCalendar = new FilmingCalendar();
            foreach (Scene scene in chromosome.Scenes) {
                int index = chromosome.Scenes.IndexOf(scene);
                if (index > start && index < end && scene.marked == false) {
                    Scene newScene = new Scene();
                    newScene.id = "0";
                    newScene.marked = true;
                    sonCalendar.Scenes.Add(newScene);
                    continue;
                }
                else sonCalendar.Scenes.Add(scene);
            }
            return sonCalendar;
        }

        public static void performCrossingPMX(int positionScenario)
        {// Se realiza el cruce de las escenas  Recalcar este metodo crea dos descendientes a la vez
            FilmingCalendar chromosome1 = movie.Scenarios[positionScenario].FilmingCalendars[0];
            FilmingCalendar chromosome2 = generateChromosome(chromosome1);
            int size = movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes.Count;
            int start = (size - size / 2) / 2;
            int end = (size - start) - 1;
            Console.WriteLine("cantidad de elementos "+chromosome1.Scenes.Count);
            Console.WriteLine("start "+ start);
            Console.WriteLine("end " + end );
            FilmingCalendar descendent1;
            FilmingCalendar descendent2;

            for (int i = 0; i <1; i++)
            { // El cruce se realizará la cantidad de veces que se ejecute este for 
              //descendent1 = chromosome1;
              //descendent2 = chromosome2;
                descendent1 = createSonChromosome(size,start,end,chromosome1);
                descendent2 = createSonChromosome(size, start, end, chromosome2);

                //FilmingCalendar newDesendent1 = changeScenes(chromosome2, descendent1);
                FilmingCalendar newDesendent2 = changeScenes(chromosome1, descendent2,end);

                //accommodateScenesInDays(newDesendent1, positionScenario);
                accommodateScenesInDays(newDesendent2, positionScenario);
                //chromosome1 = newDesendent1;
                //chromosome2 = newDesendent2;
            }
        }

        public static List<Day> chooseTheBestCalendar(int positionScenario)
        {
            List<Day> bestList = movie.Scenarios[positionScenario].Days;
            int totalcost = Data.calculatePriceOfCalendar(positionScenario, movie.Scenarios[positionScenario].Days); ;
            //int count = -1;
            //Console.WriteLine("Numero de lista de dias "+movie.Scenarios[positionScenario].possibleDays.Count);
            foreach (List<Day> days in movie.Scenarios[positionScenario].possibleDays)
            {
                int cost = Data.calculatePriceOfCalendar(positionScenario, days);
                //count += 1;
                //Console.WriteLine(" " + count);
                if (cost < totalcost)
                {
                    //Console.WriteLine("mejor " + totalcost);
                    bestList = days;
                    totalcost = cost;
                }
            }
            return bestList;
        }

        public static int availableSpace(Time Shedule)
        {// retorna cuanto espacio tiene la jornada segun sus paginas 
            int space = 0;
            foreach (Scene scene in Shedule.Scenes)
            {
                space += scene.Pages;
            }
            return Shedule.MaximunScriptPages - space;// Se le resta el espacio tatal al que ya posee, para obtener el que esta disponible 
        }

        public static bool canAssignedToActor(Day currentDay, List<Day> newDays, Scene scene)
        {// verifica el respeto de las horas a lavorar de cada actor 
            Day before;
            if (newDays.IndexOf(currentDay) != 0)
            {//contexto de jornada de dia 
                before = newDays.ElementAt(newDays.IndexOf(currentDay) - 1);// el dia anterior 
                if (scene.Schedule == true)
                {  // si la escena se realiza en el dia 
                    foreach (Scene auxScene in before.NightTime.Scenes)
                    {
                        foreach (Actor actor in auxScene.Actors)// recorre los actores del dia anterior
                        {
                            foreach (Actor auxActor in scene.Actors)// recorre los actores de la escena actual, la que se desea agregar
                            {
                                if (actor.ID.Equals(auxActor.ID))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                if (scene.Schedule == false)
                {// si la escena se realiza en la noche 
                    foreach (Scene auxScene in before.DayTime.Scenes)
                    {
                        foreach (Actor actor in auxScene.Actors)
                        {// recorre los actores del dia anterior 
                            foreach (Actor auxActor in scene.Actors)
                            {// recorre los actores de la escena actual, la que se desea agregar
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
            for (int i = 1; i <= calendar.Scenes.Count; i++)// se crean los dias para asignarle las escenas que ya fueron cruzdas  
            {
                Day newDay = new Day();
                newDay.DayNumber = i;
                Time time1 = new Time();
                time1.MaximunScriptPages = 45;
                Time time2 = new Time();
                time2.MaximunScriptPages = 45;
                newDay.DayTime = time1;
                newDay.NightTime = time2;
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

                    if (scene.Schedule == true)
                    {
                        int space = availableSpace(day.DayTime);// obtiene espacio disponible 
                        if (scene.Pages <= space)
                        {
                            bool can1 = canAssignedToActor(day, newDays, scene);// validar si los actores no estan el dia anterior en la jornada contraria 
                            if (can1 == true)
                            {
                                day.DayTime.Scenes.Add(scene);
                            }
                        }
                    }
                    if (scene.Schedule == false)
                    {
                        int space = availableSpace(day.NightTime);// obtiene espacio disponible 
                        if (scene.Pages <= space)
                        {
                            bool can2 = canAssignedToActor(day, newDays, scene);// validar si los actores no estan el dia anterior en la jornada contraria 
                            if (can2 == true)
                            {
                                day.NightTime.Scenes.Add(scene);
                            }
                        }

                    }
                }
            }
            movie.Scenarios[positionScenario].possibleDays.Add(newDays);
        }
    }
}


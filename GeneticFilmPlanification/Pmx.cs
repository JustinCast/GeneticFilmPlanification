﻿using GeneticFilmPlanification.Models;
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

        public static FilmingCalendar changeScenes(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar)
        {

            for (int k = 0; k < fatherCalendar.Scenes.Count; k++)
            {// recorre las escenas del padre 2 con el fin de asignarla a una escena del desentiente 1 que se encuentre en marcada como true 
                for (int n = 0; n < sonCalendar.Scenes.Count; n++)
                {// recorre las escenas del desenciente hasta encontrar una que este marcada como true 
                    if (sonCalendar.Scenes[n].marked == true)
                    {
                        bool exists = sonCalendar.Scenes.Contains(fatherCalendar.Scenes[k]);// verifica si existe la escena del padre en el hijo
                        if (exists == false)
                        {
                            sonCalendar.Scenes[n].marked = false;
                            sonCalendar.Scenes[n] = fatherCalendar.Scenes[k];// se le asigna a la escena que estaba en null la escena que no se encuentra en ese calendario aún
                        }
                    }
                }
            }
            return sonCalendar;
        }

        public static void performCrossingPMX(int positionScenario)
        {// Se realiza el cruce de las escenas 
            FilmingCalendar chromosome1;   // Recalcar este metodo crea dos descendientes a la vez
            FilmingCalendar chromosome2;
            chromosome1 = movie.Scenarios[positionScenario].FilmingCalendars[0];
            chromosome2 = generateChromosome(chromosome1);
            int size = movie.Scenarios[positionScenario].FilmingCalendars[0].Scenes.Count;
            int start = (size - size / 2) / 2;
            int end = (size - start) - 1;
            FilmingCalendar descendent1;
            FilmingCalendar descendent2;
            for (int i = 0; i < 5; i++)
            { // El cruce se realizará la cantidad de veces que se ejecute este for 
                descendent1 = chromosome1;
                descendent2 = chromosome2;

                for (int j = 0; j < size; j++)
                {// for que se encarga de poner en null a las escenas que se encuentren en el rango establecido de los futuros descendientes
                    if (descendent1.Scenes.IndexOf(descendent1.Scenes[j]) > start ||
                        descendent1.Scenes.IndexOf(descendent1.Scenes[j]) < end)
                    {
                        descendent1.Scenes[j].marked = true;

                    }
                    if (descendent2.Scenes.IndexOf(descendent2.Scenes[j]) > start ||
                        descendent2.Scenes.IndexOf(descendent2.Scenes[j]) < end)
                    {
                        descendent2.Scenes[j].marked = true;
                    }
                }

                FilmingCalendar newDesendent1 = changeScenes(chromosome2, descendent1);
                FilmingCalendar newDesendent2 = changeScenes(chromosome1, descendent2);


                accommodateScenesInDays(newDesendent1, positionScenario);
                accommodateScenesInDays(newDesendent2, positionScenario);
                chromosome1 = newDesendent1;
                chromosome2 = newDesendent2;
            }
        }

        public static List<Day> chooseTheBestCalendar(int positionScenario)
        {
            List<Day> bestList = null;
            int totalcost = 0;
            foreach (List<Day> days in movie.Scenarios[positionScenario].possibleDays)
            {
                int cost = Data.calculatePriceOfCalendar(positionScenario, days);
                if (bestList == null)
                {
                    bestList = days;
                    totalcost = cost;
                    continue;
                }
                if (bestList != null && cost < totalcost)
                {
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


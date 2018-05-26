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
        public static FilmingCalendar generateChromosome(FilmingCalendar calendar) {// se altera el orden de calendario para crear otro pero en orden distinto
            FilmingCalendar currentCalendar = calendar;
            FilmingCalendar newCalendar= new FilmingCalendar();
            for (int i =currentCalendar.Scenes.Count-1;i>=0;i--) {
                newCalendar.Scenes.Add(currentCalendar.Scenes[i]);
            }
            return newCalendar;
        }

        public static FilmingCalendar changeScenes(FilmingCalendar fatherCalendar, FilmingCalendar sonCalendar)
        {
            for (int k = 0; k < fatherCalendar.Scenes.Count; k++)
            {// recorre las escenas del padre 2 con el fin de asignarla a una escena del desentiente 1 que se encuentre en null 
                for (int n = 0; n < sonCalendar.Scenes.Count; n++)
                {// recorre las escenas del desenciente hasta encontrar una en null
                    if (sonCalendar.Scenes[0] == null)
                    {
                        bool exists = sonCalendar.Scenes.Contains(fatherCalendar.Scenes[k]);// verifica si existe la escena del padre en el hijo
                        if (exists == false)
                        {
                            sonCalendar.Scenes[n] = fatherCalendar.Scenes[k];// se le asigna a la escena que estaba en null la escena que no se encuentra en ese calendario aún
                        }
                    }
                }
            }
            return sonCalendar;
        }

        public static void performCrossingPMX(FilmingCalendar calendar,Movie movie, int positionScenario) {// se realiza el cruce entre los dos calendarios para obtener otro mediante un cruce
            FilmingCalendar chromosome1;                                    // Recalcar este metodo crea dos descendientes a la vez
            FilmingCalendar chromosome2;
            chromosome1 = calendar;
            chromosome2 = generateChromosome(calendar);
            int size = calendar.Scenes.Count;
            int start = (size - size / 2) / 2;
            int end = size - start;
            FilmingCalendar descendent1;
            FilmingCalendar descendent2;
            for (int i=0; i<5;i++) { // El cruce se realizará la cantidad de veces que se ejecute este for 
                descendent1 = chromosome1;
                descendent2 = chromosome2;
                for (int j = 0; j < size; j++) {// for que se encarga de poner en null a las escenas que se encuentren en el rango establecido de los futuros descendientes
                    if (descendent1.Scenes.IndexOf(descendent1.Scenes[j]) > start || 
                        descendent1.Scenes.IndexOf(descendent1.Scenes[j]) < end )
                    {
                        descendent1.Scenes[j] = null;

                    }
                    if (descendent2.Scenes.IndexOf(descendent2.Scenes[j]) > start ||
                        descendent2.Scenes.IndexOf(descendent2.Scenes[j]) < end)
                    {
                        descendent2.Scenes[j] = null;
                    }
                }
                FilmingCalendar newDesendent1= changeScenes(chromosome2, descendent1);
                FilmingCalendar newDesendent2= changeScenes(chromosome1, descendent2);
                movie.Scenarios[positionScenario].FilmingCalendars.Add(newDesendent1);
                movie.Scenarios[positionScenario].FilmingCalendars.Add(newDesendent2);
                chromosome1 = newDesendent1;
                chromosome2 = newDesendent2;
            }
        }

        public FilmingCalendar chooseTheBestCalendar(Movie movie, int positionScenario) {
            FilmingCalendar bestCalendar= movie.Scenarios[positionScenario].FilmingCalendars[0];
            foreach (FilmingCalendar calendar in movie.Scenarios[positionScenario].FilmingCalendars) {
                if (calendar.Cost>bestCalendar.Cost) {
                    bestCalendar = calendar;
                }
            }
            return bestCalendar;
        }
    }
}

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

        public static void performCrossingPMX(FilmingCalendar calendar) {// se realiza el cruce entre los dos calendarios para obtener otro mediante un cruce
            FilmingCalendar chromosome1;
            FilmingCalendar chromosome2;
            chromosome1 = calendar;
            chromosome2 = generateChromosome(calendar);
            int size = calendar.Scenes.Count;
            int start = (size - size / 2) / 2;
            int end = size - start;
            FilmingCalendar descendent1;
            FilmingCalendar descendent2;
            descendent1 = chromosome1;
            descendent2= chromosome2;
            for (int i=0; i<5;i++) { // El cruce se realizará la cantidad de veces que se ejecute este for 
                for (int j = 0; i < size; j++) {// for que se encarga de poner en null a las escenas que se encuentren en el rango establecido de los futuros descendientes
                    if (descendent1.Scenes.IndexOf(descendent1.Scenes[j]) > start || 
                        descendent1.Scenes.IndexOf(descendent1.Scenes[j]) < end )
                    {
                        descendent1.Scenes[i] = null;
                    }
                    if (descendent2.Scenes.IndexOf(descendent2.Scenes[j]) > start ||
                        descendent2.Scenes.IndexOf(descendent2.Scenes[j]) < end)
                    {
                        descendent2.Scenes[i] = null;
                    }
                }
                foreach (Scene scene in chromosome2.Scenes) {//agregarle a las scenas que se encuentren en null la escena del padre contrario 
                   //bool exists=

                }
            }
        }
    }
}

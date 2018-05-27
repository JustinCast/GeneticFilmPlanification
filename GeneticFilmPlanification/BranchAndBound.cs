using GeneticFilmPlanification.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification
{
    class BranchAndBound
    {
        private List<FilmingCalendar> FilmingCalendars = new List<FilmingCalendar>();
        private FilmingCalendar BSSF;
        int count = 0;
        public BranchAndBound(List<Scenario> scenarios)
        {
            this.InitData(scenarios);
        }
        /// <summary>
        /// Metodo que llena de datos a la lista de calendarios
        /// Estos son necesarios para hacer las combinaciones en el orden de las escenas
        /// </summary>
        /// <param name="scenarios"></param>
        private void InitData(List<Scenario> scenarios)
        {
            // la siguiente línea setea por defecto el primer calendario
            // como mejor solución
            this.BSSF = scenarios.ElementAt(0).FilmingCalendars.ElementAt(0);
            this.BSSF.Cost = FilmingCalendar.CalendarCost(BSSF.Scenes);
            foreach (Scenario s in scenarios)
            {
                this.FilmingCalendars.Add(s.FilmingCalendars.ElementAt(0));
            }
        }

        public void RunBB()
        {
            for (int i = 0; i < this.FilmingCalendars.Count; i++)
                MakeCombination(this.FilmingCalendars.ElementAt(i).Scenes);
        }
        

        private void MakeCombination(List<Scene> scenes)
        {
            Console.WriteLine("Combination");
            if (scenes.Count == 0) {
                Console.WriteLine("RECURSION COMPLETE");
                return;
            }
            foreach(Scene s in scenes)
            {
                if (FilmingCalendar.CalendarCost(scenes) > BSSF.Cost)
                { // si el costo es mayor retorna la rec
                    Console.WriteLine("Costo de las combinaciones: " + CombinationCost(scenes));
                    Console.WriteLine("Costo de la mejor solucion: " + this.BSSF.Cost);
                    return;
                }
                MakeNewBSSF(scenes);
                List<Scene> aux = ShallowClone(scenes);
                aux.Remove(s);
                Console.WriteLine("LLAMADA # "+ count);
                count++;
                MakeCombination(aux);
            }

        }

        // Return a shallow clone of a list.
        private List<T> ShallowClone<T>(List<T> items)
        {
            return new List<T>(items);
        }

        /// <summary>
        /// Creará una nueva mejor solución (calendario)
        /// </summary>
        /// <param name="scenes"></param>
        private void MakeNewBSSF(List<Scene> scenes)
        {
            this.BSSF.Scenes = scenes;
            this.BSSF.Cost = FilmingCalendar.CalendarCost(scenes);
        }

        private int CombinationCost(List<Scene> scenes)
        {
            return FilmingCalendar.CalendarCost(scenes);
        }


    }
}

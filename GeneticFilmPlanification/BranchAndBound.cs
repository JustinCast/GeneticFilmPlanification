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
        /// <summary>
        /// en esta lista se guarda el orden de la combinacion
        /// Cuando la recursividad retorne, se podrá calcular el costo de esa combinacion
        /// </summary>
        private List<Scene> Combination = new List<Scene>();
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
            {
                // clona la lista
                this.Combination = this.ShallowClone(this.FilmingCalendars.ElementAt(i).Scenes);
                MakeCombination(this.FilmingCalendars.ElementAt(i).Scenes);
            }
        }
        

        private void MakeCombination(List<Scene> scenes)
        {            
            if (scenes.Count == 0) {
                if (CombinationCost(Combination) < BSSF.Cost)
                    MakeNewBSSF(Combination);
                // limpia la lista para que guarde una nueva combinacion
                this.Combination.Clear();
                return;
            }
            foreach(Scene s in scenes)
            {
                /*
                 * Si el costo actual de la combinacion es mayor al costo de la
                 * solución actual, no tiene sentido seguir combinando
                 * IMPLEMENTACION LC-FIFO
                 * **/
                if (CombinationCost(Combination) > BSSF.Cost)
                    return;
                else
                {
                    // se quita la escena del inicio y se agrega al final
                    this.Combination.Remove(s); this.Combination.Add(s);
                    List<Scene> aux = ShallowClone(scenes);
                    aux.Remove(s);
                    PrintCombination(this.Combination);
                    MakeCombination(aux);
                }
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
            int cost = FilmingCalendar.CalendarCost(scenes);
            Console.WriteLine("COSTO COMBINACION: " + cost);
            return cost;
        }

        private void PrintCombination(List<Scene> scences)
        {
            Console.WriteLine("######### COMBINACIÓN #########");
            String c = "";
            foreach (Scene s in scences)
                c += s.Location.ID + "-";
            Console.WriteLine(c);
        }
    }
}

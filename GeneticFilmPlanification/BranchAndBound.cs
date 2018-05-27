using GeneticFilmPlanification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification
{
    class BranchAndBound
    {
        private List<FilmingCalendar> FilmingCalendars = new List<FilmingCalendar>();
        private FilmingCalendar BSSF = 
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
            foreach(Scenario s in scenarios)
            {
                this.FilmingCalendars.Add(s.FilmingCalendars.ElementAt(0))
;           }
        }

        private void MakeCombination(List<Scene> scenes)
        {
            if (scenes.Count == 0)
                return;
            foreach(Scene s in scenes)
            {
                if (CombinationCost(scenes) > BSSF.Cost) // si el costo es mayor retorna la rec
                    return;
                MakeNewBSSF(scenes);
                List<Scene> aux = scenes;
                aux.Remove(s);
                MakeCombination(aux);
            }

        }
        /// <summary>
        /// Creará una nueva mejor solución (calendario)
        /// </summary>
        /// <param name="scenes"></param>
        private void MakeNewBSSF(List<Scene> scenes)
        {

        }

        private int CombinationCost(List<Scene> scenes)
        {
            return 0;
        }


    }
}

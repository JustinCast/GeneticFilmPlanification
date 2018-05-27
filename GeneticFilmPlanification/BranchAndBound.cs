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
            foreach(Scenario s in scenarios)
            {
                this.FilmingCalendars.Add(s.FilmingCalendars.ElementAt(0))
;           }
        }

        private void MakeCombination(List<Scene> scenes)
        {
            if (scenes.Count == 0)
                return;

        }


    }
}

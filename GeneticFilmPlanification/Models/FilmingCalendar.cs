using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class FilmingCalendar
    {
        public List<Scene> Scenes = new List<Scene>();
        public int Cost { get; set; }


        /// <summary>
        /// Calcula el costo total del calendario
        /// </summary>
        /// <returns>Costo total del calendario</returns>
        public int CalendarCost()
        {
            this.Cost = 0;
            foreach(Scene s in Scenes)
            {
                Cost += s.Act
            }
            return 0;
        }
    }
}

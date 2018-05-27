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

        FilmingCalendar() { }

        FilmingCalendar(List<Scene> scenes, int cost)
        {
            this.Scenes = scenes;
            this.Cost = cost;
        }


        /// <summary>
        /// Calcula el costo total del calendario
        /// </summary>
        /// <returns>Costo total del calendario</returns>
        public static int CalendarCost(List<Scene> scenes)
        {
            int cost = 0;
            foreach(Scene s in scenes)
                foreach (Actor a in s.Actors)
                    cost += a.CostPerDay;
            return cost;
        }
    }
}

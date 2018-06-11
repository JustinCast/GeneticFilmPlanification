using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Scene
    {
        // se calcula en base a las páginas que la escena ocupa en el guión
        public int Duration { get; set; }
        public int Pages { get; set; }
        public List<Actor> Actors = new List<Actor>(); 
        public Location Location { get; set; }
        public bool Schedule { get; set; } // true = dia, false = noche
        public int DayNumber { get; set; }
        public bool assigned = false;
        public bool marked = false;// se utiliza en el metodo PMX
        public String id;


        public int SceneMemoryCost()
        {
            int cost = 0;
            // duration
            cost += 4;
            // pages
            cost += 4;
            foreach (Actor a in Actors)
            {
                // costPerDay
                cost += 4;
                // FirstParticipation
                cost += 4;
                // LastParticipation
                cost += 4;
                // ID
                cost += a.ID.Length;
            }
            // Location
            // ID
            cost += Location.ID.Length;
            // InUse
            cost++;
            //
            // Schedule
            cost++;
            // DayNumber
            cost += 4;
            // asigned
            cost++;
            // marked
            cost++;
            // id
            cost += id.Length;
            return cost;
        }
    }
}

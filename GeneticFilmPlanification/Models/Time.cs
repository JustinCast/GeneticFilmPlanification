using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Time  
    {
        public int MaximunScriptPages { get; set; } // duración máxima establecida
        public List<Scene> Scenes = new List<Scene>();
        public List<Location> AvailableLocations = new List<Location>();

        public int GetMemoryCost()
        {
            int cost = 0;
            // MaximunScriptPages
            cost += 4;
            foreach(Scene s in Scenes)
            {
                // duration
                cost += 4;
                // pages
                cost += 4;
                foreach(Actor a in s.Actors)
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
                // location
                cost += s.Location.ID.Length;
                // inUse
                cost += 1;
                // Schedule
                cost += 1;
                // DayNumber
                cost += 4;
                // assigned
                cost += 1;
                // marked
                cost += 1;
                // id
                cost += s.id.Length;
            }
            foreach(Location l in AvailableLocations)
            {
                // ID
                cost += l.ID.Length;
                // InUse
                cost += 1;
            }
            return cost;
        }

        public bool IfSceneIsAllowed(Scene s)
        {
            int cost = 0;
            foreach(Scene sc in Scenes)
            {
                cost += sc.Pages;
            }
            return (cost + s.Pages) < MaximunScriptPages;
        }

        public int TotalPages()
        {
            int cost = 0;
            Scenes.ForEach(s => cost += s.Pages);
            return cost;
        }

        public bool IfLocationIsUsed(Location l)
        {
            foreach (Scene s in Scenes)
                if (s.Location.Equals(l))
                {
                    return true;
                }
            return false;
        }

        public void UseLocation(Location l)
        {
            foreach(Location loc in AvailableLocations)
            {
                if (loc == l)
                    loc.InUse = true;
            }
        }
    }
}

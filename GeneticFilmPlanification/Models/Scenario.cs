using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Scenario
    {
        public List<FilmingCalendar> FilmingCalendars = new List<FilmingCalendar>();
        public List<Actor> Actors = new List<Actor>();
        public List<Location> Locations = new List<Location>(); 
        public List<Day> Days = new List<Day>();
        public int StageNumber { get; set; } // necesario para poder localizar el escenario

        public List<List<Day>> possibleDays = new List<List<Day>>();// PMX = se ingresaran listas con los posibles dias en diferentes ordenes en el que se realizaran las escenas

        public int MemoryCostForBB()
        {
            int cost = 0;
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

            foreach (Location l in Locations)
            {
                // ID
                cost += l.ID.Length;
                // InUse
                cost += 1;
            }
            foreach (Day d in Days)
                cost += d.GetDayMemoryCost();

            // StageNumber
            cost += 4;
            return cost;
        }
    }
}

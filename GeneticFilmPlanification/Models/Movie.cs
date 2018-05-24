using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Movie
    {
        public List<Actor> Actors { get; set; }   
        public List<Location> Locations { get; set; }
        public List<FilmingCalendar> FilmingCalendars { get; set; }


        public int GetActorCost(String id)
        {
            foreach(Actor a in Actors)
            {
                if (a.Equals(id))
                    return a.CostPerDay;
            }
            return 0;
        }
    }
}


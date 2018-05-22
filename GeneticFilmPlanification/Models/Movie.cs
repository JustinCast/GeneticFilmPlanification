using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Movie
    {
        List<Actor> Actors { get; set; }   
        List<Location> Locations { get; set; }
        List<FilmingCalendar> FilmingCalendars { get; set; }
    }
}


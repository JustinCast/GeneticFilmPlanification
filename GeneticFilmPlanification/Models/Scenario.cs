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

        public List<List<Day>> possibleDays = new List<List<Day>>();// PMX = se ingresaran listas con los posibles dias en diferentes ordenes en el que se realizaran las escenas
    }
}

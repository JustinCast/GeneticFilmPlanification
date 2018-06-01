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
        public FilmingCalendar FilmingCalendarBestSolutionsB_B;

        public List<Actor> Actors = new List<Actor>();
        public List<Location> Locations = new List<Location>(); 
        public List<Day> Days = new List<Day>();
        public int StageNumber { get; set; } // necesario para poder localizar el escenario

        public List<List<Day>> possibleDays = new List<List<Day>>();// PMX = se ingresaran listas con los posibles dias en diferentes ordenes en el que se realizaran las escenas

        /*public Day GetDay()*/
    }
}

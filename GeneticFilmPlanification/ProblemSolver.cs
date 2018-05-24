using GeneticFilmPlanification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification
{
    class ProblemSolver
    {
        private class MovieOptimizationSolution
        {
            public List<FilmingCalendar> Calendars;

            MovieOptimizationSolution(List<FilmingCalendar> calendars)
            {
                this.Calendars = calendars;
            }
            
            public double CalendarCost()
            {
                return 0.0;
            }
        }
    }
}

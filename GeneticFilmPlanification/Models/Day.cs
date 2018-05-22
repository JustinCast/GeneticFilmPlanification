using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Day
    {
        public Time DayTime { get; set; }
        public Time NightTime { get; set; }
        public int DayNumber { get; set; }
    }
}

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
    }
}

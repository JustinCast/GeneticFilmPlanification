using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Movie
    {
        private static Movie Instance = null;
        public List<Scenario> Scenarios = new List<Scenario>();

        private Movie() { }

        public static Movie GetInstance()
        {
            if (Instance == null)
                Instance = new Movie();
            return Instance;
        }

        public Scenario GetScenarioByNumber(int number)
        {
            return Scenarios.Find(s => s.StageNumber == number);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticFilmPlanification.Models;

namespace GeneticFilmPlanification
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.createScenario(5, 40, 10, 1, 4);
            Data.createScenario(6, 60, 15, 2, 3);
            Data.createScenario(7, 100, 17, 3, 5);
            Data.createScenario(8, 80, 20, 4, 4);
            Data.printScenarios();
            Console.ReadKey();
        }
    }
}

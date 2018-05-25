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
            Data.createScenario1(5,40,0);// location ,actors, positionCalendar
            Data.createScenario2(6, 60, 1);// location ,actors, positionCalendar
            Data.printScenarios();
            Console.ReadKey();
        }
    }
}

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
        static Movie movie = Movie.GetInstance();
        static void Main(string[] args)
        {

            Data.createScenariosOfMovie();
            Data.createScenario1(5, 40, 0);// location ,actors, posicion del escenario
            Data.createScenario2(6, 60, 1);// location ,actors, posicion del escenario
            Data.createScenario3(7, 100, 2);// location ,actors, posicion del escenario
            Data.createScenario4(8, 80, 3);// location ,actors, posicion del escenario
            Data.printScenarios();

            Data.createDays();
            Data.assignScenesToDay();
            Data.assignLocationsToDay();
            
           

            Data.performPmxInAllScenarios();
            Pmx.clearLists();
            Pmx.performOxInAllScenarios();

            

            Console.ReadKey();
        }
    }
}
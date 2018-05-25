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
            Movie movie = new Movie();
            Data.createScenariosOfMovie(movie);

            Data.createScenario1(5,40, movie,0);// location ,actors, pelicula, posicion del escenario
            Data.createScenario2(6, 60, movie,1);// location ,actors, pelicula, posicion del escenario
            Data.createScenario3(7, 100, movie,2);// location ,actors, pelicula, posicion del escenario
            Data.createScenario4(8, 80, movie,3);// location ,actors, pelicula, posicion del escenario
            Data.printScenarios(movie);

            Data.createDays(movie);

            Data.assignScenesToDay(movie,0);// pelicula, posicion del escenario
            Data.assignScenesToDay(movie,1);
            Data.assignScenesToDay(movie,2);
            Data.assignScenesToDay(movie,3);

            Console.ReadKey();
        }
    }
}

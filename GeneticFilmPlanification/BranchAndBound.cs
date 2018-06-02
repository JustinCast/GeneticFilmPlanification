using GeneticFilmPlanification.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification
{
    class BranchAndBound
    {
        // calendarios de cada escenario
        private List<FilmingCalendar> Scenario1Calendars = new List<FilmingCalendar>();
        private List<FilmingCalendar> Scenario2Calendars = new List<FilmingCalendar>();
        private List<FilmingCalendar> Scenario3Calendars = new List<FilmingCalendar>();
        private List<FilmingCalendar> Scenario4Calendars = new List<FilmingCalendar>();

        // mejores soluciones para cada calendario
        private FilmingCalendar BSSF1;
        private FilmingCalendar BSSF2;
        private FilmingCalendar BSSF3;
        private FilmingCalendar BSSF4;
        private int CurrentFilmingCalendarID = 0;
        /// <summary>
        /// en esta lista se guarda el orden de la combinacion
        /// Cuando la recursividad retorne, se podrá calcular el costo de esa combinacion
        /// </summary>
        private List<Scene> Combination = new List<Scene>();
        public BranchAndBound(List<Scenario> scenarios, Movie movieInstance)
        {
            // seteo por defecto de las mejores soluciones para cada escenario
            this.BSSF1 = scenarios.ElementAt(0).FilmingCalendars.ElementAt(0);
            this.BSSF1.Cost = CombinationCost(0);
            this.BSSF2 = scenarios.ElementAt(1).FilmingCalendars.ElementAt(0);
            this.BSSF2.Cost = CombinationCost(1);
            this.BSSF3 = scenarios.ElementAt(2).FilmingCalendars.ElementAt(0);
            this.BSSF3.Cost = CombinationCost(2);
            this.BSSF4 = scenarios.ElementAt(3).FilmingCalendars.ElementAt(0);
            this.BSSF4.Cost = CombinationCost(3);
            // inicialización de datos para el algoritmo
            this.Scenario1Calendars.Add(scenarios[0].FilmingCalendars.ElementAt(0));
            this.Scenario2Calendars.Add(scenarios[1].FilmingCalendars.ElementAt(0));
            this.Scenario3Calendars.Add(scenarios[2].FilmingCalendars.ElementAt(0));
            this.Scenario4Calendars.Add(scenarios[3].FilmingCalendars.ElementAt(0));
        }

        /// <summary>
        /// Por cada escenario se hace una combinación con el fin
        /// de obtener el mejor calendario en relación al costo
        /// </summary>
        public void RunBB()
        {
            // Escenario 1
            this.Combination = this.ShallowClone(this.Scenario1Calendars.ElementAt(0).Scenes);
            MakeCombination(this.Scenario1Calendars.ElementAt(0).Scenes, BSSF1, 0);
            // Escenario 2
            this.Combination = this.ShallowClone(this.Scenario2Calendars.ElementAt(0).Scenes);
            MakeCombination(this.Scenario2Calendars.ElementAt(0).Scenes, BSSF2, 1);
            // Escenario 3
            this.Combination = this.ShallowClone(this.Scenario3Calendars.ElementAt(0).Scenes);
            MakeCombination(this.Scenario3Calendars.ElementAt(0).Scenes, BSSF3, 2);
            // Escenario 4
            this.Combination = this.ShallowClone(this.Scenario4Calendars.ElementAt(0).Scenes);
            MakeCombination(this.Scenario4Calendars.ElementAt(0).Scenes, BSSF4, 3);
            //this.CurrentFilmingCalendarID = this.FilmingCalendars.ElementAt(i).AssignedScenario;
        }
        

        private void MakeCombination(List<Scene> scenes, FilmingCalendar BSSF, int pos)
        {            
            if (scenes.Count == 0) {
                if (CombinationCost(pos) < BSSF.Cost)
                    MakeNewBSSF(Combination, BSSF, pos);
                // limpia la lista para que guarde una nueva combinacion
                this.Combination.Clear();
                return;
            }
            foreach(Scene s in scenes)
            {
                /*
                 * Si el costo actual de la combinacion es mayor al costo de la
                 * solución actual, no tiene sentido seguir combinando
                 * IMPLEMENTACION LC-FIFO
                 * **/
                if (CombinationCost(pos) >= BSSF.Cost)
                    return;
                else
                {
                    // se quita la escena del inicio y se agrega al final
                    this.Combination.Remove(s); this.Combination.Add(s);
                    List<Scene> aux = ShallowClone(scenes);
                    aux.Remove(s);
                    PrintCombination(this.Combination);
                    MakeCombination(aux, BSSF, pos);
                }
            }
        }

        // Return a shallow clone of a list.
        private List<T> ShallowClone<T>(List<T> items)
        {
            return new List<T>(items);
        }

        /// <summary>
        /// Creará una nueva mejor solución (calendario)
        /// </summary>
        /// <param name="scenes"></param>
        private void MakeNewBSSF(List<Scene> scenes, FilmingCalendar BSSF, int position)
        {
            BSSF.Scenes = scenes;
            BSSF.Cost = CombinationCost(position);
        }

        private int CombinationCost(int position)
        {
            int cost = Data.calculatePriceOfCalendar(position, Movie.GetInstance().Scenarios[position].Days);
            Console.WriteLine("COSTO COMBINACION: " + cost);
            return cost;
        }

        private void PrintCombination(List<Scene> scenes)
        {
            Console.WriteLine("######### COMBINACIÓN #########");
            String c = "";
            foreach (Scene s in scenes)
                c += s.Location.ID + "-";
            Console.WriteLine(c);
        }

        /// <summary>
        /// Método que valida si una combinación de escenas es válida
        /// Teniendo en cuenta validaciones como:
        /// - Disponibilidad de Actores
        /// - Disponibilidad de Localizaciones
        /// - Verificación de Jornadas
        /// 
        /// Este metodo revisara desde el día 1 hasta el último día, esto con el fin
        /// de encontrar un espacio disponible en alguno de estos días
        /// </summary>
        /// <param name="combination"></param>
        /// <returns></returns>
        /*private bool ValidateCombination(List<Scene> combination, int FC_ID)
        {
            foreach(Scenario s in )
            return false;
        }*/
    }
}

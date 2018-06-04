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
        #region variables necesarias para el algoritmo B&B
        // calendarios de cada escenario
        private List<FilmingCalendar> Scenario1Calendars = new List<FilmingCalendar>();
        private List<FilmingCalendar> Scenario2Calendars = new List<FilmingCalendar>();
        private List<FilmingCalendar> Scenario3Calendars = new List<FilmingCalendar>();
        private List<FilmingCalendar> Scenario4Calendars = new List<FilmingCalendar>();

        // mejores soluciones para cada calendario
        public List<Day> BSSF1 = new List<Day>();
        public List<Day> BSSF2 = new List<Day>();
        public List<Day> BSSF3 = new List<Day>();
        public List<Day> BSSF4   = new List<Day>();
        private int InitialCost1, InitialCost2, InitialCost3, InitialCost4;
        private int FinalCost1, FinalCost2, FinalCost3, FinalCost4;
        /// <summary>
        /// en esta lista se guarda el orden de la combinacion
        /// Cuando la recursividad retorne, se podrá calcular el costo de esa combinacion
        /// </summary>
        private List<Scene> Combination = new List<Scene>();
        private bool FirstTime = true;
        #endregion

        #region seteo o configuración del algoritmo
        public BranchAndBound(List<Scenario> scenarios, Movie movieInstance)
        {
            // seteo por defecto de las mejores soluciones para cada escenario
            this.BSSF1 = scenarios[0].Days;
            InitialCost1 = Data.calculatePriceOfCalendar(0, BSSF1);

            this.BSSF2 = scenarios[1].Days;
            InitialCost2 = Data.calculatePriceOfCalendar(1, BSSF2);

            this.BSSF3 = scenarios[2].Days;
            InitialCost3 = Data.calculatePriceOfCalendar(2, BSSF3);

            this.BSSF4 = scenarios[3].Days;
            InitialCost4 = Data.calculatePriceOfCalendar(3, BSSF4);

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
            FirstTime = true;
            this.Combination = this.ShallowClone(this.Scenario2Calendars.ElementAt(0).Scenes);
            MakeCombination(this.Scenario2Calendars.ElementAt(0).Scenes, BSSF2, 1);
            // Escenario 3
            FirstTime = true;
            this.Combination = this.ShallowClone(this.Scenario3Calendars.ElementAt(0).Scenes);
            MakeCombination(this.Scenario3Calendars.ElementAt(0).Scenes, BSSF3, 2);
            // Escenario 4
            FirstTime = true;
            this.Combination = this.ShallowClone(this.Scenario4Calendars.ElementAt(0).Scenes);
            MakeCombination(this.Scenario4Calendars.ElementAt(0).Scenes, BSSF4, 3);
            //this.CurrentFilmingCalendarID = this.FilmingCalendars.ElementAt(i).AssignedScenario;
        }
        #endregion

        #region ramificación de B&B
        private void MakeCombination(List<Scene> scenes, FilmingCalendar BSSF, int pos)
        {            
            if (scenes.Count == 0) {
                if(EvaluateCombination(scenes, pos))
                    if (CombinationCost(Combination, pos) < BSSF.Cost)
                        MakeNewBSSF(Combination, BSSF, pos);
                // limpia la lista para que guarde una nueva combinacion
                //this.Combination.Clear();
                //return;
            }
            foreach(Scene s in scenes)
            {
                /*
                 * Si el costo actual de la combinacion es mayor al costo de la
                 * solución actual, no tiene sentido seguir combinando
                 * IMPLEMENTACION LC-FIFO
                 * **/
                if (!FirstTime)
                    if (CombinationCost(Combination, pos) >= BSSF.Cost)
                        return; //poda
                
                FirstTime = false;
                // se quita la escena del inicio y se agrega al final
                this.Combination.Remove(s); this.Combination.Add(s);
                List<Scene> aux = ShallowClone(scenes);
                aux.Remove(s);
                PrintCombination(this.Combination);
                MakeCombination(aux, BSSF, pos);
                
            }
        }
        #endregion

        #region región de código para poda o evaluación del algoritmo
        /// <summary>
        /// Creará una nueva mejor solución (calendario)
        /// </summary>
        /// <param name="scenes"></param>
        private void MakeNewBSSF(List<Scene> scenes, FilmingCalendar BSSF, int pos)
        {
            BSSF.Scenes = scenes;
            BSSF.Cost = CombinationCost(scenes, pos);
        }

        private int CombinationCost(List<Scene> scenes, int pos)
        {
            int cost = CalendarCost(scenes);
            Console.WriteLine("COSTO COMBINACION: " + cost);
            return cost;
        }

        /// <summary>
        /// Calcula el costo total del calendario para el Algoritmo B&B
        /// </summary>
        /// <returns>Costo total del calendario</returns>
        private int CalendarCost(List<Scene> scenes)
        {
            int cost = 0;
            foreach(Scene s in scenes)
            {
                foreach (Actor a in s.Actors)
                    cost += a.CostPerDay;
            }
            return cost;
        }

        private bool EvaluateCombination(List<Scene> scenes, int pos)
        {
            foreach (Day d in Movie.GetInstance().Scenarios[pos].Days)
            {
                foreach (Scene s in scenes)
                {
                    if (d.DayTime.IfSceneIsAllowed(s) && !d.DayTime.IfLocationIsUsed(s.Location))
                    {
                        foreach (Actor a in s.Actors)
                        {
                            if (CheckActorOverloadInDifferentDays(a, pos) || CheckActorOverloadInSameDay(a, pos))
                            {
                                return false;
                            }
                        }
                    }
                    else
                        return false;
                }
            }
            return true;
        }

        #endregion

        #region región de código donde se tienen todas las impresiones en consola
        public void PrintCostComparison()
    {
        Console.WriteLine("________________________________________");
        Console.WriteLine("Costo INICIAL Calendario 1: " + InitialCost1);
        Console.WriteLine("Costo FINAL Calendario 1: " + BSSF1.Cost);
        Console.WriteLine("________________________________________");
        Console.WriteLine("Costo INICIAL Calendario 2: " + InitialCost2);
        Console.WriteLine("Costo FINAL Calendario 2: " + BSSF2.Cost);
        Console.WriteLine("________________________________________");
        Console.WriteLine("Costo INICIAL Calendario 3: " + InitialCost3);
        Console.WriteLine("Costo FINAL Calendario 3: " + BSSF3.Cost);
        Console.WriteLine("________________________________________");
        Console.WriteLine("Costo INICIAL Calendario 4: " + InitialCost4);
        Console.WriteLine("Costo FINAL Calendario 4: " + BSSF4.Cost);
        Console.WriteLine("________________________________________");
    }

        private void PrintCombination(List<Scene> scenes)
        {
            Console.WriteLine("######### COMBINACIÓN #########");
            String c = "";
            foreach (Scene s in scenes)
                c += s.id + "-";
            Console.WriteLine(c);
        }

        #endregion

        #region región de código para método auxiliares
        // Return a shallow clone of a list.
        private List<T> ShallowClone<T>(List<T> items)
        {
            return new List<T>(items);
        }


        /// <summary>
        /// Método que se encarga de revisar si un actor tiene una sobrecarga de trabajo en días distintos
        /// </summary>
        /// <param name="a"></param>
        /// <param name="scenarioPos"></param>
        /// <returns></returns>
        private bool CheckActorOverloadInDifferentDays(Actor a, int scenarioPos)
        {
            for(int i = 0; i < Movie.GetInstance().Scenarios[scenarioPos].Days.Count(); i++)
            {
                for(int j = (i + 1); j < Movie.GetInstance().Scenarios[scenarioPos].Days.Count(); j++)
                {
                    // verifica si hay 1 día de diferencia para verificar la sobrecarga
                    if(
                        (Movie.GetInstance().Scenarios[scenarioPos].Days[j].DayNumber -
                        Movie.GetInstance().Scenarios[scenarioPos].Days[i].DayNumber) == 1)
                    {
                        /* verifica si participa en la jornada de día un día específico
                         * y si trabaja en la jornada de noche al siguiente día
                         */
                        if (
                            IfParticipateInTime(Movie.GetInstance().Scenarios[scenarioPos].Days[i].DayTime, a)
                            &&
                            IfParticipateInTime(Movie.GetInstance().Scenarios[scenarioPos].Days[j].NightTime, a))
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Método que verifica si el actor participa en dos jornadas el  mismo día
        /// </summary>
        /// <param name="a"></param>
        /// <param name="scenarioPos"></param>
        /// <returns></returns>
        private bool CheckActorOverloadInSameDay(Actor a, int scenarioPos)
        {
            for(int i = 0; i < Movie.GetInstance().Scenarios[scenarioPos].Days.Count(); i++)
            {
                if (
                    IfParticipateInTime(Movie.GetInstance().Scenarios[scenarioPos].Days[i].DayTime, a)
                    &&
                    IfParticipateInTime(Movie.GetInstance().Scenarios[scenarioPos].Days[i].NightTime, a))
                    return true;
            }
            return false;
        }

        private bool IfParticipateInTime(Time time, Actor a)
        {
            foreach (Scene s in time.Scenes)
                if (s.Actors.Contains(a))
                    return true;
            return false;
        }
        #endregion

    }
}

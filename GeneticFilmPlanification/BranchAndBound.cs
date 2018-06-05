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
        // listas de cada escenario
        private List<Scene> Scenario1Scenes = new List<Scene>();
        private List<Scene> Scenario2Scenes = new List<Scene>();
        private List<Scene> Scenario3Scenes = new List<Scene>();
        private List<Scene> Scenario4Scenes = new List<Scene>();

        public List<Day> Scenario1Days = new List<Day>();
        public List<Day> Scenario2Days = new List<Day>();
        public List<Day> Scenario3Days = new List<Day>();
        public List<Day> Scenario4Days = new List<Day>();

        // BSSF
        public List<Scene> BSSF1 = new List<Scene>();
        public List<Scene> BSSF2 = new List<Scene>();
        public List<Scene> BSSF3 = new List<Scene>();
        public List<Scene> BSSF4 = new List<Scene>();

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
            this.Scenario1Days = scenarios[0].Days;
            InitialCost1 = Data.calculatePriceOfCalendar(0, Scenario1Days);

            this.Scenario2Days = scenarios[1].Days;
            InitialCost2 = Data.calculatePriceOfCalendar(1, Scenario2Days);

            this.Scenario3Days = scenarios[2].Days;
            InitialCost3 = Data.calculatePriceOfCalendar(2, Scenario3Days);

            this.Scenario4Days = scenarios[3].Days;
            InitialCost4 = Data.calculatePriceOfCalendar(3, Scenario4Days);

            // inicialización de datos para el algoritmo
            AssignDayAtrribute(Scenario1Days, Scenario1Scenes);
            AssignDayAtrribute(Scenario2Days, Scenario2Scenes);
            AssignDayAtrribute(Scenario3Days, Scenario3Scenes);
            AssignDayAtrribute(Scenario4Days, Scenario4Scenes);
        }

        /// <summary>
        /// Por cada escenario se hace una combinación con el fin
        /// de obtener el mejor calendario en relación al costo
        /// </summary>
        public void RunBB()
        {
            // Escenario 1
            this.Combination = this.ShallowClone(Scenario1Scenes);
            SetActorParticipation(Movie.GetInstance().Scenarios[0].Actors, Scenario1Scenes);
            MakeCombination(Scenario1Scenes, Scenario1Days, BSSF1);
            // Escenario 2
            FirstTime = true;
            this.Combination = this.ShallowClone(Scenario2Scenes);
            SetActorParticipation(Movie.GetInstance().Scenarios[1].Actors, Scenario2Scenes);
            MakeCombination(Scenario2Scenes, Scenario2Days, BSSF2);
            // Escenario 3
            FirstTime = true;
            this.Combination = this.ShallowClone(Scenario3Scenes);
            SetActorParticipation(Movie.GetInstance().Scenarios[2].Actors, Scenario3Scenes);
            MakeCombination(Scenario3Scenes, Scenario3Days, BSSF3);
            // Escenario 4
            FirstTime = true;
            this.Combination = this.ShallowClone(Scenario4Scenes);
            SetActorParticipation(Movie.GetInstance().Scenarios[3].Actors, Scenario4Scenes);
            MakeCombination(Scenario4Scenes, Scenario4Days, BSSF4);
            //this.CurrentFilmingCalendarID = this.FilmingCalendars.ElementAt(i).AssignedScenario;
        }
        #endregion

        #region ramificación de B&B
        private void MakeCombination(List<Scene> scenes, List<Day> days, List<Scene> BSSF)
        {   if (!FirstTime)
            {
                if (EvaluateCombination(scenes, days))
                {
                    if (CombinationCost(scenes) < CombinationCost(BSSF))
                    {
                        MakeNewBSSF(scenes, BSSF);
                        return;
                    }
                    // si es mayor o igual se poda
                    return;
                }
                // si la combinación no es válida, se poda
                return;
            }
            foreach(Scene s in scenes)
            {
                FirstTime = false;
                if(s.Schedule)
                {
                    s.Schedule = false; // se llama nuevamente al método con la jornada de noche
                    MakeCombination(scenes, days, BSSF);
                }
                else // se llama nuevamente al método con la jornada de noche
                {
                    s.Schedule = true; // se llama nuevamente al método con la jornada de día
                    MakeCombination(scenes, days, BSSF);
                }
                /*
                 * El siguiente fragmento cambia el # día y jornada de una escena y llama nuevamente al método
                 * Va cambiando el número de día desde el 1 hasta tamaño - 1
                 * **/
                for(int i = 1; i < days[days.Count - 1].DayNumber; i++)
                {
                    s.DayNumber = i;
                    MakeCombination(scenes, days, BSSF);
                }
            }
        }
        #endregion

        #region región de código para poda o evaluación del algoritmo
        /// <summary>
        /// Creará una nueva mejor solución (calendario)
        /// </summary>
        /// <param name="scenes"></param>
        private void MakeNewBSSF(List<Scene> scenes, List<Scene> BSSF)
        {
            BSSF = scenes;
        }

        private int CombinationCost(List<Scene> scenes)
        {
            int total = 0;
            List<Actor> AlreadyCalculated = new List<Actor>();
            foreach (Scene s in scenes)
                foreach (Actor a in s.Actors)
                    total += ((a.LastParticipation - a.FirstParticipation) + 1) * a.CostPerDay;
            Console.WriteLine("TOTAL: " + total);
            return total;
        }

        private void SetActorParticipation(List<Actor> actors, List<Scene> scenes)
        {
            foreach (Actor a in actors)
                foreach (Scene s in scenes)
                    foreach (Actor ac in s.Actors)
                    {
                        if (ac.Equals(a) && a.FirstParticipation == 0)
                        {
                            ac.FirstParticipation = s.DayNumber;
                            break;
                        }
                        if(ac.Equals(a))
                        {
                            ac.LastParticipation = s.DayNumber;
                            break;
                        }
                    }
        }

        /// <summary>
        /// Calcula el costo total del calendario para el Algoritmo B&B
        /// </summary>
        /// <returns>Costo total del calendario</returns>
        private int CalendarCost(int pos, List<Day> days)
        {
            return Data.calculatePriceOfCalendar(pos, days);
        }

        private bool EvaluateCombination(List<Scene> scenes, List<Day> days)
        {
            foreach (Day d in days)
            {
                foreach (Scene s in scenes)
                {
                    if (d.DayTime.IfSceneIsAllowed(s) && !d.DayTime.IfLocationIsUsed(s.Location))
                    {
                        foreach (Actor a in s.Actors)
                        {
                            if (CheckActorOverloadInDifferentDays(a, days) || CheckActorOverloadInSameDay(a, days))
                            {
                                return false;
                            }
                        }                        
                    }
                    else
                        return false;
                }
            }
            Console.WriteLine("PASÓ LA PRUEBA");
            return true;
        }

        #endregion

        #region región de código donde se tienen todas las impresiones en consola
        public void PrintCostComparison()
        {
            Console.WriteLine("________________________________________");
            Console.WriteLine("Costo INICIAL Calendario 1: " + InitialCost1);
            Console.WriteLine("Costo FINAL Calendario 1: " + CombinationCost(BSSF1));
            Console.WriteLine("________________________________________");
            Console.WriteLine("Costo INICIAL Calendario 2: " + InitialCost2);
            Console.WriteLine("Costo FINAL Calendario 2: " + CombinationCost(BSSF2));
            Console.WriteLine("________________________________________");
            Console.WriteLine("Costo INICIAL Calendario 3: " + InitialCost3);
            Console.WriteLine("Costo FINAL Calendario 3: " + CombinationCost(BSSF3));
            Console.WriteLine("________________________________________");
            Console.WriteLine("Costo INICIAL Calendario 4: " + InitialCost4);
            Console.WriteLine("Costo FINAL Calendario 4: " + CombinationCost(BSSF4));
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

        private void AssignDayAtrribute(List<Day> days, List<Scene> scenes)
        {
            foreach (Day d in days)
            {
                foreach (Scene s in d.DayTime.Scenes)
                {
                    s.DayNumber = d.DayNumber;
                    scenes.Add(s);
                }
                foreach (Scene s in d.NightTime.Scenes)
                {
                    s.DayNumber = d.DayNumber;
                    scenes.Add(s);
                }
            }
        }

        /// <summary>
        /// Método que verifica si una escena está contenida en una jornada
        /// </summary>
        /// <param name="time"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool CheckIfTheSceneIsInTheTime(Time time, Scene s)
        {
            return time.Scenes.Contains(s);
        }

        /// <summary>
        /// Método que se encarga de revisar si un actor tiene una sobrecarga de trabajo en días distintos
        /// </summary>
        /// <param name="a"></param>
        /// <param name="scenarioPos"></param>
        /// <returns></returns>
        private bool CheckActorOverloadInDifferentDays(Actor a, List<Day> days)
        {
            for(int i = 0; i < days.Count(); i++)
            {
                for(int j = (i + 1); j < days.Count(); j++)
                {
                    // verifica si hay 1 día de diferencia para verificar la sobrecarga
                    if(
                        (days[j].DayNumber -
                        days[i].DayNumber) == 1)
                    {
                        /* verifica si participa en la jornada de día un día específico
                         * y si trabaja en la jornada de noche al siguiente día
                         */
                        if (
                            IfParticipateInTime(days[i].DayTime, a)
                            &&
                            IfParticipateInTime(days[j].NightTime, a))
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
        private bool CheckActorOverloadInSameDay(Actor a, List<Day> days)
        {
            for(int i = 0; i < days.Count(); i++)
            {
                if (
                    IfParticipateInTime(days[i].DayTime, a)
                    &&
                    IfParticipateInTime(days[i].NightTime, a))
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

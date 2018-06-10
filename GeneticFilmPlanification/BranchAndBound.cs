using GeneticFilmPlanification.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification
{
    class BranchAndBound
    {
        #region variables necesarias para el algoritmo B&B
        // contadores de asignaciones y comparaciones
        static int countA = 0;
        static int countC = 0;
        static int memorySize = 0;

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
        public List<int> BSSF = new List<int>();
        static int cont = 0;

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
            this.Scenario1Days = scenarios[0].Days; countA++;
            InitialCost1 = Data.calculatePriceOfCalendar(0, Scenario1Days); countA++;

            this.Scenario2Days = scenarios[1].Days; countA++;
            InitialCost2 = Data.calculatePriceOfCalendar(1, Scenario2Days); countA++;

            this.Scenario3Days = scenarios[2].Days; countA++;
            InitialCost3 = Data.calculatePriceOfCalendar(2, Scenario3Days); countA++;

            this.Scenario4Days = scenarios[3].Days; countA++;
            InitialCost4 = Data.calculatePriceOfCalendar(3, Scenario4Days); countA++;

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
            cont = 0; countA++;
            this.Combination = this.ShallowClone(Scenario1Scenes); countA++;
            SetActorParticipation(Movie.GetInstance().Scenarios[0].Actors, Scenario1Scenes);
            Console.WriteLine("|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°| ESCENARIO 1 |°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|\n\n");
            MakeCombination(Scenario1Scenes, Scenario1Days, InitialCost1);
            FinalCost1 = BSSF.Min(); countA++;
            BSSF.Clear();
            memorySize += Movie.GetInstance().Scenarios[0].MemoryCostForBB();
            BranchAndBound.ScenariosResults();
            countA = 0; countC = 0; cont = 0;

            // Escenario 2
            memorySize = 0;
            cont = 0; countA++;
            FirstTime = true; countA++;
            this.Combination = this.ShallowClone(Scenario2Scenes); countA++;
            Console.WriteLine("|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°| ESCENARIO 2 |°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|\n\n");
            SetActorParticipation(Movie.GetInstance().Scenarios[1].Actors, Scenario2Scenes);
            MakeCombination(Scenario2Scenes, Scenario2Days, InitialCost2);
            FinalCost2 = BSSF.Min(); countA++;
            BSSF.Clear(); countA++;
            memorySize += Movie.GetInstance().Scenarios[1].MemoryCostForBB();
            BranchAndBound.ScenariosResults();
            countA = 0; countC = 0; cont = 0;

            // Escenario 3
            memorySize = 0;
            cont = 0; countA++;
            FirstTime = true; countA++;
            this.Combination = this.ShallowClone(Scenario3Scenes); countA++;
            SetActorParticipation(Movie.GetInstance().Scenarios[2].Actors, Scenario3Scenes);
            Console.WriteLine("|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°| ESCENARIO 3 |°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|\n\n");
            MakeCombination(Scenario3Scenes, Scenario3Days, InitialCost3);
            FinalCost3 = BSSF.Min(); countA++;
            BSSF.Clear();
            memorySize += Movie.GetInstance().Scenarios[2].MemoryCostForBB();
            BranchAndBound.ScenariosResults();
            countA = 0; countC = 0; cont = 0;

            // Escenario 4
            memorySize = 0;
            cont = 0; countA++;
            FirstTime = true; countA++;
            this.Combination = this.ShallowClone(Scenario4Scenes); countA++;
            SetActorParticipation(Movie.GetInstance().Scenarios[3].Actors, Scenario4Scenes);
            Console.WriteLine("|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°| ESCENARIO 4 |°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|\n\n");
            MakeCombination(Scenario4Scenes, Scenario4Days, InitialCost4);
            FinalCost4 = BSSF.Min(); countA++;
            BSSF.Clear(); countA++;
            memorySize += Movie.GetInstance().Scenarios[3].MemoryCostForBB();
            BranchAndBound.ScenariosResults();
            countA = 0; countC = 0; cont = 0;

            PrintCostComparison();
        }
        #endregion

        #region ramificación de B&B
        private void MakeCombination(List<Scene> scenes, List<Day> days, int initialCost)
        {            
            countC++;
            if (scenes.Count() == 0)
            {
                countC++;
                if (EvaluateCombination(Combination, days))
                {
                    countC +=2;
                    if (CombinationCost(Combination) < initialCost && CombinationCost(Combination) > 0)
                    {
                        //PrintCombination(Combination);
                        BSSF.Add(CombinationCost(Combination)); countA++; memorySize += 4;
                        return;
                    }
                    return;
                }
                return;
            }
            countA++;
            foreach (Scene s in scenes)
            {
                countC +=3;
                if (!FirstTime)
                    if (CombinationCost(Combination) > initialCost || cont == 5000)
                        return;
                FirstTime = false; countA++; memorySize++;
                this.Combination.Remove(s); this.Combination.Add(s); countA+=2;
                List<Scene> aux = ShallowClone(scenes); countA++;  memorySize += s.SceneMemoryCost() * aux.Count();
                aux.Remove(s);
                SetActorParticipation(s.Actors, Combination);
                cont++;
                MakeCombination(aux, days, initialCost);
            }
        }
        #endregion

        #region región de código para poda o evaluación del algoritmo
        private int CombinationCost(List<Scene> scenes)
        {
            int total = 0; countA++; memorySize += 4;
            List<Actor> AlreadyCalculated = new List<Actor>(); countA++;
            countA++;
            memorySize += scenes[0].SceneMemoryCost();
            foreach (Scene s in scenes)
            {
                countA++;
                memorySize += s.Actors[0].ActorMemoryCost();
                foreach (Actor a in s.Actors)
                {
                    countC++;
                    if (!AlreadyCalculated.Contains(a))
                    {
                        total += ((a.LastParticipation - a.FirstParticipation) + 1) * a.CostPerDay; countA++; memorySize += 4;
                        AlreadyCalculated.Add(a); memorySize += a.ActorMemoryCost();
                    }
                }
            }
            return total;
        }

        private void SetActorParticipation(List<Actor> actors, List<Scene> scenes)
        {
            countA++;
            memorySize += actors[0].ActorMemoryCost();
            foreach (Actor a in actors)
            {
                countA++;
                memorySize += scenes[0].SceneMemoryCost();
                foreach (Scene s in scenes)
                {
                    countA++;
                    memorySize += actors[0].ActorMemoryCost();
                    foreach (Actor ac in s.Actors)
                    {
                        countC += 2; countA+=2;
                        if (ac.Equals(a) && a.FirstParticipation == 0)
                        {
                            ac.FirstParticipation = s.DayNumber; memorySize += 4;
                            break;
                        }
                        countC++;
                        if (ac.Equals(a))
                        {
                            ac.LastParticipation = s.DayNumber; memorySize += 4;
                            break;
                        }
                    }
                }
            }
        }

        private bool EvaluateCombination(List<Scene> scenes, List<Day> days)
        {
            countA++;
            memorySize += days[0].GetDayMemoryCost();
            foreach (Day d in days)
            {
                countA++;
                memorySize += scenes[0].SceneMemoryCost();
                foreach (Scene s in scenes)
                {
                    countC+=2;
                    if (d.DayTime.IfSceneIsAllowed(s) && !d.DayTime.IfLocationIsUsed(s.Location))
                    {
                        countA++;
                        memorySize += s.Actors[0].ActorMemoryCost();
                        foreach (Actor a in s.Actors)
                        {
                            countC += 2;
                            if (CheckActorOverloadInDifferentDays(a, days) || CheckActorOverloadInSameDay(a, days))
                            {
                                return false;
                            }
                        }
                    }                  
                    else if (d.NightTime.IfSceneIsAllowed(s) && !d.NightTime.IfLocationIsUsed(s.Location))
                    {
                        countC += 2;
                        countA++;
                        memorySize += s.Actors[0].ActorMemoryCost();
                        foreach (Actor a in s.Actors)
                        {
                            countC += 2;
                            if (CheckActorOverloadInDifferentDays(a, days) || CheckActorOverloadInSameDay(a, days))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
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
            Console.WriteLine("Costo FINAL Calendario 1: " + FinalCost1);
            Console.WriteLine("________________________________________");
            Console.WriteLine("Costo INICIAL Calendario 2: " + InitialCost2);
            Console.WriteLine("Costo FINAL Calendario 2: " + FinalCost2);
            Console.WriteLine("________________________________________");
            Console.WriteLine("Costo INICIAL Calendario 3: " + InitialCost3);
            Console.WriteLine("Costo FINAL Calendario 3: " + FinalCost3);
            Console.WriteLine("________________________________________");
            Console.WriteLine("Costo INICIAL Calendario 4: " + InitialCost4);
            Console.WriteLine("Costo FINAL Calendario 4: " + FinalCost4);
            Console.WriteLine("________________________________________");
        }

        public static void ScenariosResults()
        {
            Console.WriteLine("Cantidad de asignaciones: " + countA);
            Console.WriteLine("Cantidad de comparaciones: " + countC);
            Console.WriteLine("Cantidad de bytes utilizados: " + memorySize);
            Console.WriteLine("Cantidad de llamadas recursivas: " + cont);
        }

        private void PrintCombination(List<Scene> scenes)
        {
            Console.WriteLine("######### COMBINACIÓN #########");
            String c = ""; countA++;
            countA++;
            foreach (Scene s in scenes)
            {
                c += s.id + "-"; countA++; memorySize += s.id.Length + 1;
            }
            Console.WriteLine(c);
        }

        #endregion

        #region región de código para método auxiliares
        // Return a shallow clone of a list.
        private List<T> ShallowClone<T>(List<T> items)
        {
            countA++;
            return new List<T>(items);
        }

        private void AssignDayAtrribute(List<Day> days, List<Scene> scenes)
        {
            countA++;
            memorySize += days[0].GetDayMemoryCost();
            foreach (Day d in days)
            {
                countA++;
                foreach (Scene s in d.DayTime.Scenes)
                {
                    s.DayNumber = d.DayNumber; countA++; memorySize += 4;
                    scenes.Add(s); countA++; memorySize += s.SceneMemoryCost();
                }
                countA++;
                foreach (Scene s in d.NightTime.Scenes)
                {
                    s.DayNumber = d.DayNumber; countA++; memorySize += 4;
                    scenes.Add(s); countA++; memorySize += s.SceneMemoryCost();
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
            countC++;
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
            countA++; //primera asignación del for
            countC++; // ultima comparación falsa del for
            memorySize += 4;
            for (int i = 0; i < days.Count(); i++)
            {
                // i increment
                memorySize += 4;
                countC++; // comparación del 'i' en cada iteración
                countA++; // aumento del 'i' en cada iteración
                countA++; //asignación del 'j' en cada iteración
                countC++; // comparación falsa de 'j'
                // j assignment
                memorySize += 4;
                for (int j = (i + 1); j < days.Count(); j++)
                {
                    // j increment
                    memorySize += 4;
                    // verifica si hay 1 día de diferencia para verificar la sobrecarga
                    countC++;
                    if (
                        (days[j].DayNumber -
                        days[i].DayNumber) == 1)
                    {
                        /* verifica si participa en la jornada de día un día específico
                         * y si trabaja en la jornada de noche al siguiente día
                         */
                        countC+=2;
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
            countA++; // primera asignación 'i'
            countC++; // comparación falsa
            // i assignment
            memorySize += 4;
            for(int i = 0; i < days.Count(); i++)
            {
                // i increment
                memorySize += 4;
                countC++; //comparación del 'i' en cada iteración
                countC+=2;
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
            countA++;
            foreach (Scene s in time.Scenes)
            {
                countC++;
                if (s.Actors.Contains(a))
                    return true;
            }
            return false;
        }
        #endregion

    }
}

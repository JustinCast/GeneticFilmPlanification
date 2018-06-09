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
        // contadores de asignaciones y comparaciones
        int countA = 0;
        int countC = 0;

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
        int cont = 0;

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
            InitialCost1 = Data.calculatePriceOfCalendar(1, Scenario2Days);

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
            cont = 0;
            this.Combination = this.ShallowClone(Scenario1Scenes);
            SetActorParticipation(Movie.GetInstance().Scenarios[0].Actors, Scenario1Scenes);
            Console.WriteLine("|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°| ESCENARIO 1 |°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|\n\n");
            MakeCombination(Scenario1Scenes, Scenario1Days, InitialCost1);
            FinalCost1 = BSSF.Min();
            BSSF.Clear();
            // Escenario 2
            cont = 0;
            FirstTime = true;
            this.Combination = this.ShallowClone(Scenario2Scenes);
            Console.WriteLine("|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°| ESCENARIO 2 |°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|\n\n");
            SetActorParticipation(Movie.GetInstance().Scenarios[1].Actors, Scenario2Scenes);
            MakeCombination(Scenario2Scenes, Scenario2Days, InitialCost2);
            FinalCost2 = BSSF.Min();
            BSSF.Clear();
            // Escenario 3
            cont = 0;
            FirstTime = true;
            this.Combination = this.ShallowClone(Scenario3Scenes);
            SetActorParticipation(Movie.GetInstance().Scenarios[2].Actors, Scenario3Scenes);
            Console.WriteLine("|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°| ESCENARIO 3 |°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|\n\n");
            MakeCombination(Scenario3Scenes, Scenario3Days, InitialCost3);
            FinalCost3 = BSSF.Min();
            BSSF.Clear();
            // Escenario 4
            cont = 0;
            FirstTime = true;
            this.Combination = this.ShallowClone(Scenario4Scenes);
            //SetActorParticipation(Movie.GetInstance().Scenarios[3].Actors, Scenario4Scenes);
            Console.WriteLine("|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°| ESCENARIO 4 |°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|°|\n\n");
            MakeCombination(Scenario4Scenes, Scenario4Days, InitialCost4);
            FinalCost4 = BSSF.Min();
            BSSF.Clear();

            PrintCostComparison();
        }
        #endregion

        #region ramificación de B&B
        private void MakeCombination(List<Scene> scenes, List<Day> days, int initialCost)
        {           
            if (scenes.Count() == 0)
            {
                if (EvaluateCombination(Combination, days))
                {
                    if (CombinationCost(Combination) < initialCost && CombinationCost(Combination) > 0)
                    {
                        //Console.WriteLine(CombinationCost(Combination));
                        BSSF.Add(CombinationCost(Combination));                        
                        return;
                    }
                    return;
                }
                return;
            }
            foreach(Scene s in scenes)
            {
                if (!FirstTime)
                    if (CombinationCost(Combination) > initialCost || cont == 2000)
                        return;
                FirstTime = false;
                this.Combination.Remove(s); this.Combination.Add(s);
                List<Scene> aux = ShallowClone(scenes);
                aux.Remove(s);
                SetActorParticipation(s.Actors, Combination);
                cont++;
                MakeCombination(aux, days, initialCost);
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
                {
                    if (!AlreadyCalculated.Contains(a))
                    {
                        total += ((a.LastParticipation - a.FirstParticipation) + 1) * a.CostPerDay;
                        AlreadyCalculated.Add(a);
                    }
                }
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
                    else if (d.NightTime.IfSceneIsAllowed(s) && !d.NightTime.IfLocationIsUsed(s.Location))
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
        private void PrintScenes(List<Day> days)
        { 
            foreach(Day d in days)
            {
                Console.WriteLine("Jornada día");
                foreach (Scene s in d.DayTime.Scenes)
                    Console.WriteLine(s.Location.ID);
                Console.WriteLine("Jornada noche");
                foreach (Scene s in d.NightTime.Scenes)
                    Console.WriteLine(s.Location.ID);
            }
        }

        // Return a shallow clone of a list.
        private List<T> ShallowClone<T>(List<T> items)
        {
            return new List<T>(items);
        }

        private List<Day> TempPopulate(List<Scene> scenes, List<Day> days)
        {
            List<Day> temp = ShallowClone(days);
            for(int i = 1; i < temp.Count(); i++)
                foreach(Scene s in scenes)
                    if (s.DayNumber == i) {
                        if (s.Schedule)
                        {
                            if (!temp.ElementAt(i).DayTime.Scenes.Contains(s))
                            {
                                temp.ElementAt(i).DayTime.Scenes.Add(s);
                                temp.ElementAt(i).DayTime.MaximunScriptPages += s.Duration;
                            }
                        }
                        else
                        {
                            if (!temp.ElementAt(i).NightTime.Scenes.Contains(s))
                            {
                                temp.ElementAt(i).NightTime.Scenes.Add(s);
                            }
                        }
                    }
            /*Console.WriteLine("Lista despúes de agregación");
            PrintScenes(temp);*/
            return temp;
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
                        {
                            Console.WriteLine("Sobrecarga en el día " + days[i].DayNumber);
                            Console.WriteLine("ID actor: " + a.ID);
                            return true;
                        }
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
                {
                    Console.WriteLine("Sobrecarga en el día " + days[i].DayNumber);
                    Console.WriteLine("ID actor: " + a.ID);
                    return true;
                }
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

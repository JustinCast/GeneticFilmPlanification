using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFilmPlanification.Models
{
    class Actor
    {
        public int CostPerDay { get; set; }
        public int FirstParticipation = 0;
        public int LastParticipation = 0;
        public String ID { get; set; }

        public int ActorMemoryCost()
        {
            int cost = 0;
            // CostPerDay
            cost += 4;
            // FirstParticipation
            cost += 4;
            // LastParticipation
            cost += 4;
            // ID 
            cost += ID.Length;
            return cost;
        }
    }
}

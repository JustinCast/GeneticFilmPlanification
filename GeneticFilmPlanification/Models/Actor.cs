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
    }
}

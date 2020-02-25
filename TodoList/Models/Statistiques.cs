using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    public class Statistiques
    {
        public int NbTachesTerminees { get; set; }
        public int NbTachesEnCours { get; set; }
        public int NbTachesRetard { get; set; }
        public int MoyenneTpsTaches { get; set; }
    }
}

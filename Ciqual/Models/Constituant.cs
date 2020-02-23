using System;
using System.Collections.Generic;

namespace Ciqual.Models
{
    public partial class Constituant
    {
        public Constituant()
        {
            Composition = new HashSet<Composition>();
        }

        public int IdConstituant { get; set; }
        public string Nom { get; set; }
        public string Unite { get; set; }

        public virtual ICollection<Composition> Composition { get; set; }
    }
}

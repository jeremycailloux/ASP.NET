using System;
using System.Collections.Generic;

namespace Ciqual.Models
{
    public partial class Aliment
    {
        public Aliment()
        {
            Composition = new HashSet<Composition>();
        }

        public int IdAliment { get; set; }
        public string Nom { get; set; }
        public int IdFamille { get; set; }

        public virtual Famille IdFamilleNavigation { get; set; }
        public virtual ICollection<Composition> Composition { get; set; }
    }
}

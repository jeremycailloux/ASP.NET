using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ciqual.Models
{
    public class AlimentConsti
    {   
        [Display(Name ="Identifiant")]
        public int IdAliment { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        public int IdFamille { get; set; }
        [Display(Name = "Constituants")]
        public int NbConstituant{ get; set; }
        public virtual Famille IdFamilleNavigation { get; set; }

        public AlimentConsti(int idAliment, string nom, int idFamille, int nbConstituant)
        {
            IdAliment = idAliment;
            Nom = nom;
            IdFamille = idFamille;
            NbConstituant = nbConstituant;
        }
    }
}

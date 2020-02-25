using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    // créer une classe POCO nommée Tache avec les propriétés suivantes : Id(entier), Description, DateCreation, DateEcheance, Terminee(booléen)

    public class Tache :IValidatableObject
    {
        
        [Key]
        public int id { get; set; }

        // Description soit obligatoire, avec une longueur maximale de 250 caractères
        [Display(Name = "Description")]
        [StringLength(250), Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Date de creation")]
        [DataType(DataType.Date)] /* ou [DisplayFormat(DataFormatString = "{dd/MM/yyyy}", ApplyFormatInEditMode = true)] */
        public DateTime DateCreation { get; set; }

        // Date d’échéance soit facultative: DateTime?, on doit alors faire un add-migration depuis la console, puis update database
        [Display(Name = "Date d'echeance")]
        [DataType(DataType.Date)] /* [DisplayFormat(DataFormatString = "{dd/MM/yyyy}", ApplyFormatInEditMode = true)] */
        public DateTime? DateEcheance { get; set; }
        public bool Terminee { get; set; }

        // Générer la base de données à partir du modèle créé précédemment, en utilisant les commandes Add-Migration et Update-Database de la console du gestionnaire de package NuGet. Attention à bien sélectionner le bon projet dans cette dernière !

        // Créer une règle de validation côté serveur pour vérifier que la date d’échéance est supérieure à la date de création de la tâche
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Tache tache = (Tache)validationContext.ObjectInstance;
            if (tache.DateCreation > tache.DateEcheance)
            {
                yield return new ValidationResult("La date de création ne peut pas être postérieure à la date d'échéance");
                yield return new ValidationResult("*", new string[] { "DateCreation", "DateEcheance" });
            }
        }
    }
}
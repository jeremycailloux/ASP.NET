using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    // Entité de modèle nommée Calcul qui représentera les données nécessaires à l’utilitaire
    public class Calcul
    {
        // Utiliser des attributs d'affichage sur les propriétés de l’entité Calcul pour que les zones de saisies s’adaptent aux types de données
        [DataType(DataType.Date)] // On spécifie un attribut DataType qui va permettre à ASP.NET la manière dont il doit afficher cette propriété dans un champ input
        public DateTime BaseDate { get; set; }

        [Range(1, 9999, ErrorMessage = "Le nombre de jours ajouté doit être compris entre 1 et 9999")]
        [Required] // Si l’utilisateur saisit un nombre de jours qui n’est pas compris entre 1 et 9999, afficher un message d’erreur en-dessous du formulaire, et ne pas calculer le résultat.
        public int AddDays { get; set; }

        // Faire en sorte que la date de création et la date d’échéance des tâches soient saisies et affichées sans les heures. Il suffit d'utiliser l'attribut d'affichage DataType
        [DataType(DataType.Date)]
        public DateTime FinalDate { get; set; }

        public string Operator { get; set; }
    }
}

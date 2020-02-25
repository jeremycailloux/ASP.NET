/*

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    public class DateEcheanceAttribute : ValidationAttribute
    {
        private DateTime _dateEcheance;

        public DateEcheanceAttribute(DateTime DateEcheance)
        {
            _dateEcheance = DateEcheance;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Tache tache = (Tache)validationContext.ObjectInstance;

            if(tache.DateEcheance > tache.DateCreation)
            {
                return new ValidationResult("La date d'échéance doit être supérieure à la date de création");
            }

            return ValidationResult.Success;
        }

    }
}

*/

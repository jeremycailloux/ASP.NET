using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class UtilitairesController : Controller
    {
        public IActionResult Index()
        {
            // Dans l’action Index, instancier une entité Calcul permettant d’initialiser la date de la première zone de saisie à la date du jour. Générer la réponse de la même façon que dans AjouterJours

            var calcul = new Calcul();
            calcul.BaseDate = DateTime.Now;
            calcul.FinalDate = calcul.BaseDate.AddDays(calcul.AddDays);
            return View(calcul);
        }

        // Ajouter une action nommée AjouterJours qui prend en paramètres un objet Calcul. Faire en sorte qu’on puisse l’appeler aussi par l’url « localhost:xxxx/calculsdates »
        /*
        [ActionName("calculdates")]
        public IActionResult AddDays(Calcul calcul)
        {
            ModelState.Remove("FinalDate");
            // ModelState représente l'état de la liaison de modèle (liaison entre la source de données de la vue et les paramètres de l’action) 
            // après saisie des valeurs par l'utilisateur. Il doit être rafraîchi pour que le résultat de l'opération s'affiche. 
            // On peut forcer ce rafraichissement en vidant le ModelState complètement ou seulement la propriété correspondant à la donnée à rafraîchir.
            // Rafraichi les données du Model Binding
            // Sans le modelState le résultat ne s’affiche pas

            calcul.FinalDate = calcul.BaseDate.AddDays(calcul.AddDays);

            return View("Index", calcul);  
        }
        */

        // Faire en sorte que le résultat de l’opération soit désormais transmis à la vue par le ViewBag (ou ViewData) et non plus par l’entité Calcul. On mettra le code créé précédemment en commentaire pour pouvoir y revenir par la suite.
        // Dans la vue, utiliser l’attribut value au lieu de asp-for pour afficher la valeur
        [ActionName("calculdates")]
        public IActionResult AddDays(Calcul calcul)
        {
            if (ModelState.IsValid) 
            { 
                ModelState.Remove("FinalDate");
                // ModelState représente l'état de la liaison de modèle (liaison entre la source de données de la vue et les paramètres de l’action) 
                // après saisie des valeurs par l'utilisateur. Il doit être rafraîchi pour que le résultat de l'opération s'affiche. 
                // On peut forcer ce rafraichissement en vidant le ModelState complètement ou seulement la propriété correspondant à la donnée à rafraîchir.
                // Rafraichi les données du Model Binding
                ViewData["FinalDate"] = calcul.BaseDate.AddDays(calcul.AddDays);
                if(calcul.Operator == "+")
                {
                    ViewData["FinalDate"] = calcul.BaseDate.AddDays(calcul.AddDays);
                }
                if(calcul.Operator == "-")
                {
                    ViewData["FinalDate"] = calcul.BaseDate.AddDays(-calcul.AddDays);
                }
                    return View("Index");
            }

            return View("Index");
        }

        public FileResult Download()
        {
            string fileName = "calendrierdelannee.rar";
            byte[] fileBytes = System.IO.File.ReadAllBytes($"wwwroot/Files/{fileName}");
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
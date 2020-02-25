using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // On définit en constante la clé de l’info à mémoriser
        const string SessionKeyNom = "_nbVistes";

        public static int nbVisites;


        public HomeController(ILogger<HomeController> logger)
        {
            nbVisites++;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Ajouter un paramètre « id » de type entier à l’action Contact, et modifier son code pour que la vue affiche la valeur du paramètre
        // Faire en sorte que l’action Contact ne puisse être appelée que par le nom « ContactUs » dans l’url de la requête, et que la page correspondante s’affiche toujours correctement avec l'alias [ActionName("ContactUs")]

        [ActionName("ContactUs")] // On popuvait mettre Route, ActioName est un alias qui permet de simplifier les routes
        public IActionResult Contact(int id, string nom)
        {
            // Ajouter un second paramètre « nom » à l’action Contact, et faire en sorte que la vue affiche « Bonjour xxx, ton id est yyy »

            ViewData["Nom"] = "Bonjour " + nom;
            ViewData["Message"] = "Votre Id est: " + id.ToString();
            return View("Contact");
        }

        // Dans une méthode du contrôleur, on mémorise l’info en session 
        public IActionResult About()
        {
            // On souhaite afficher le nombre de fois où le visiteur est venu sur cette page au cours de la dernière minute.
            // Implémenter ceci en mémorisant le nombre de visites dans une variable statique sur le contrôleur. 
            // Cette solution fonctionne mais n'est pas bonne
            // Utiliser une variable de session à la place de la variable statique pour compter le nombre de visites de la page A propos

            int? nbVisites = HttpContext.Session.GetInt32(SessionKeyNom);
            if (nbVisites != null) {
                HttpContext.Session.SetInt32(SessionKeyNom, (int)nbVisites + 1); // nbVisites.Value + 1)
                // On définit la variable de la session ayant pour nom la variable de SessionKeyNom déclarée en haut
                // On lui passe en paramètre la variable de session existante +1 
                // On vérifie qu'elle ne soit pas nulle
                nbVisites = HttpContext.Session.GetInt32(SessionKeyNom);
                // On rappel nbVisites et on lui passe à nouveau la valeur actualisé
            }
            else
            {   // Si c'est le cas le cas on lui attribut la valeur 1
                HttpContext.Session.SetInt32(SessionKeyNom, 1);
                nbVisites = HttpContext.Session.GetInt32(SessionKeyNom);
            }

            ViewData["Message"] = "Vous avez visité cette page " + nbVisites + " fois la page About au cours de la dernière minute";
            // Lié au @ViewData["Message"] dans la vue About
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


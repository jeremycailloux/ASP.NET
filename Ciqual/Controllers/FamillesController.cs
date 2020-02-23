using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ciqual.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ciqual.Controllers
{
    public class FamillesController : Controller
    {
        private readonly CiqualContext _context;

        public FamillesController(CiqualContext context)
        {
            _context = context;
        }
        // GET: Familles
        public async Task<IActionResult> Index(string sortOrder)
        {
            IQueryable<Famille> reqFamilles = _context.Famille;

            // si la chaîne de tri reçue en paramère est vide,
            // on définit un critère de tri par défaut
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "Id";

            // On applique le tri à la requête
            switch (sortOrder)
            {
                case "Nom":
                    reqFamilles = reqFamilles.OrderBy(f => f.Nom);
                    break;
                case "Nom_desc":
                    reqFamilles = reqFamilles.OrderByDescending(f => f.Nom);
                    break;
                case "Id":
                    reqFamilles = reqFamilles.OrderBy(f => f.IdFamille);
                    break;
                case "Id_desc":
                    reqFamilles = reqFamilles.OrderByDescending(f => f.IdFamille);
                    break;
            }

            // Pour chaque critère, on envoie le sens du tri inverse
            // à la vue pour le prochain appel de la méthode
            ViewData["TriParNom"] = sortOrder == "Nom" ? "Nom_desc" : "Nom";
            ViewData["TriParId"] = sortOrder == "Id" ? "Id_desc" : "Id";
            // On récupère les données triées
            var familles = await reqFamilles.AsNoTracking().ToListAsync();

            return View(familles);
        }
    }
}
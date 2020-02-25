using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Controllers
{
    // Ajouter un contrôleur de type « Contrôleur MVC avec vues utilisant EF », qui utilise la classe Tache comme modèle et le contexte de données qu’on vient de créer. Laisser la case « Générer des vues » cochée.

    public class TachesController : Controller
    {
        private readonly TodoListContext _context;

        public TachesController(TodoListContext context)
        {
            _context = context;
        }

        // GET: Taches
        public async Task<IActionResult> Index(int etat, string recherche)
        {   // On souhaite appeler la modale depuis la liste des tâches. Or la liste utilise une liste de tâches comme modèle, tandis que la modale utilisera une tâche unique
            // On va donc faire en sorte d’ajouter systématiquement un élément vide à la fin de la liste des tâches pour le réserver comme modèle de la fenêtre modale
            // Dans l’action Index du contrôleur, avant de renvoyer la liste des tâches, ajouter une tâche vide à la fin de la liste (initialiser simplement sa date de création à la date du jour)
            // Pour se faire on cré une nouvelle variable appelé tache que l'on initialise à la date du jour et que l'on injecte dans return View
            // Dans la vue Index, faire en sorte que cette tâche ne soit jamais affichée, ni comptabilisée dans les compteurs
            // var tache = await _context.Tache.ToListAsync();
            // tache.Add(new Tache() { DateCreation = DateTime.Now });
            // return View(await _context.Tache.ToListAsync());

            IQueryable<Tache> request= _context.Tache; 
            

            // Dans la vue de la liste des tâches, entre le titre et le tableau, ajouter les éléments de filtres suivants: La zone de texte permet de filtrer les tâches dont la description contient le texte saisi et liste déroulante permet de filtrer les tâches selon leur état (terminées, non terminées, toutes)
            // Valeurs pour la liste déroulante Tâches
            var dicTaches = new Dictionary<int, string>()
            {
               { 1, "En cours" },
               { 2, "Terminée" },
               { 3, "Toutes" }
            };

            if(etat == 0)
            {
                // Lecture d’une valeur dans un cookie
                if (Request.Cookies.TryGetValue("etatfiltre", out string val))
                {
                    int.TryParse(val, out etat);
                }
            }

            // Ecriture d’une valeur dans un cookie
            var options = new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(5) // Le cookie expirera dans 5 minutes 
            };
            Response.Cookies.Append("etatfiltre", etat.ToString(), options);

            // Mémorise / transmet les valeurs de la liste, ainsi que la valeur sélectionnée
            ViewBag.Etat = new SelectList(dicTaches, "Key", "Value", etat);

            // Filtrer
            if(recherche != null)
            {
                if (etat == 1)
                {
                    request = request.Where(t => t.Terminee == false && t.Description.Contains(recherche));
                }


                if (etat == 2)
                {
                    request = request.Where(t => t.Terminee == true && t.Description.Contains(recherche));
                }
            }

            else if(recherche == null)
            {
                if (etat == 1)
                {
                    request = request.Where(t => t.Terminee == false);
                }


                if (etat == 2)
                {
                    request = request.Where(t => t.Terminee == true);
                }
            }

                List<Tache> list= await request.ToListAsync();
                list.Add(new Tache() { DateCreation = DateTime.Now }); // On remet l'ajout de la tâche vide utilisée dans le formaulaire du modal
                return View(list);
        }

        // GET: Taches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tache = await _context.Tache
                .FirstOrDefaultAsync(m => m.id == id);
            if (tache == null)
            {
                return NotFound();
            }

            return View(tache);
        }

        // GET: Taches/Create
        public IActionResult Create()
        {
            return View();
        }

        // Ajouter une entrée « Statistiques » dans le menu principal de l’application, qui appellera une action de même nom dans le contrôleur des tâches
        public async Task<IActionResult> Statistiques()
        {
            var taches = await _context.Tache.ToListAsync();

            var stats = new Statistiques();
            stats.NbTachesTerminees = taches.Where(p => p.Terminee == true).Count();
            stats.NbTachesEnCours = taches.Where(p => p.Terminee == false).Count();
            stats.NbTachesRetard = taches.Where(p => p.DateEcheance < DateTime.Now).Count();

            foreach(var item in taches)
            {
                TimeSpan datediff = (TimeSpan)(item.DateEcheance - item.DateCreation);
                stats.MoyenneTpsTaches = stats.MoyenneTpsTaches + datediff.Days;
            }

            stats.MoyenneTpsTaches = stats.MoyenneTpsTaches / taches.Count();
            // Créer une nouvelle vue « Statistiques » pour le contrôleur des tâches et faire en sorte qu’elle affiche les informations ci-dessus dans de simples libellés
            return View("Statistiques", stats);
        }

        // POST: Taches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Description,DateCreation,DateEcheance,Terminee")] Tache tache)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tache);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tache);
        }
        public IActionResult InitSaisie()
        {
            return View("CreationGroupee");
        }

        // Dans la vue CreationGroupee, créer le formulaire permettant de saisir le nombre de tâches à créer. Ce formulaire doit appeler une action InitSaisie de type Post 
        // Pour passer le nombre de tâches saisie à l’action, il suffit d’affecter à la zone de saisie un nom (propriété name) identique à celui du paramètre de l’action
        [HttpPost]
        public IActionResult InitSaisie(int nbTaches)
        {
            // Instancier une liste de x tâches (x étant la valeur reçue en paramètre) dont la date d’échéance est à la date du jour + 1 semaine
            // Retourner la vue CreationGroupee en lui passant cette liste de tâches. Ceci nous permettra d’initialiser le tableau de tâches vides
            List<Tache> list = new List<Tache>();
            for (int i = 0; i < nbTaches; i++)
            {
                list.Add(new Tache() { DateEcheance = DateTime.Now.AddDays(7) });
            }
            return View("CreationGroupee", list);
        }

        [HttpPost]
        public async Task<IActionResult> CreationGroupee([Bind("Id,Description,DateEcheance")] List<Tache> taches)
        {
            // Dans l’action CreationGroupee, enregistrer les tâches en initialisant leur date de création à la date du jour et en s’inspirant du code de l’action Create
            // Renvoyer la vue Index comme résultat pour rediriger vers la liste des tâches.

            if (ModelState.IsValid)
            {
                foreach (var tache in taches)
                {
                    tache.DateCreation = DateTime.Now;
                    _context.Add(tache);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("CreationGroupee", taches);
        }

        // Afficher le formaulaire d'édition en Get
        // GET: Taches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tache = await _context.Tache.FindAsync(id);
            if (tache == null)
            {
                return NotFound();
            }
            return View(tache);
        }

        // Editer une action et la poster
        // POST: Taches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Description,DateCreation,DateEcheance,Terminee")] Tache tache)
        {
            if (id != tache.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tache);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TacheExists(tache.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tache);
        }

        // GET: Taches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tache = await _context.Tache
                .FirstOrDefaultAsync(m => m.id == id);
            if (tache == null)
            {
                return NotFound();
            }

            return View(tache);
        }

        // POST: Taches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tache = await _context.Tache.FindAsync(id);
            _context.Tache.Remove(tache);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TacheExists(int id)
        {
            return _context.Tache.Any(e => e.id == id);
        }
    }
}

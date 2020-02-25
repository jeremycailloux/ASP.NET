using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    // 	Ajouter un constructeur au contexte, afin qu’il puisse recevoir les options de configuration, contenant la chaîne de connexion

    public class TodoListContext : DbContext
    {
        public DbSet<Tache>Tache { get; set; }
        // Créer une classe TodoListContext dérivée de DbContext, et contenant un DbSet de tâches
        public TodoListContext(DbContextOptions<TodoListContext> options) : base(options)
        {
            // Ajouter un constructeur au contexte, afin qu’il puisse recevoir les options de configuration, contenant la chaîne de connexion 
        }
    }
}

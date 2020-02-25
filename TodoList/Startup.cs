using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoList.Models;

namespace TodoList
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // Paramétrer les sessions dans la classe startup
            // Dans la classe Startup, enregistrer le contexte dans la liste des services, et lui passer la chaine de connexion
            services.AddDbContext<TodoListContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("TodoListConnect")));

            // Mise en place des sessions au moyen d'un cache mémoire
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Définit la durée maxi d'inactivité de la session
                options.IdleTimeout = TimeSpan.FromMinutes(1);

                // Active le cookie de session et le rend obligatoire
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // middleware
            // services.AddCors();
            // Mise en place des sessions au moyen d'un cache mémoire
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Définit la durée maxi d'inactivité de la session
                options.IdleTimeout = TimeSpan.FromMinutes(1);

                // Active le cookie de session et le rend obligatoire
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // Utilisation des sessions
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Pour utiliser les actions d'une autre API
            // app.UseCors(builder => builder.WithOrigins(http:\\machin.com));
        }
    }
}

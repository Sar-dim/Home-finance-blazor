using Domain.Entity;
using Infractructure.Services.Implimintations;
using Infractructure.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using net_core_backend.Database;
using net_core_backend.Models;
using net_core_backend.Repositories.Implimintations;
using net_core_backend.Repositories.Interfaces;
using net_core_backend.Services.Implimintations;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            string connection = _configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection));
            services.AddHttpContextAccessor();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<AuthService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<IBaseRepository<Operation>, BaseRepository<Operation>>();
            services.AddScoped<IBaseRepository<Person>, BaseRepository<Person>>();
            services.AddScoped<IBaseRepository<OperationSource>, BaseRepository<OperationSource>>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

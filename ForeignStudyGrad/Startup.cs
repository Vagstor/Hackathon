using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForeignStudyGrad.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using DataModels;
using DbMigrator;

namespace ForeignStudyGrad
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
            services.AddRazorPages();
            LinqToDB.Data.DataConnection.DefaultSettings = new MySettings
            {
                //ConnString = Configuration.GetConnectionString("DefaultConnection")
                ConnString = "server=ASKOLD-��\\sqlexpress;database=foreignstudy;Trusted_Connection=True;"
            };

            services.AddScoped<ForeignstudyDB>();
            services.AddScoped<AccountService>();
            services.AddScoped<CourseService>();
            //services.AddScoped<HttpService>();
            services.AddSingleton<IConfiguration>(Configuration);
            //services.AddSingleton<MonitoringService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Welcome/Login");
                    options.AccessDeniedPath = new PathString("/Welcome/Login");
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
                app.UseExceptionHandler("/Welcome/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //MigrationRunner migrationRunner = new MigrationRunner();
            //var connStr = Configuration["ConnectionStrings:DefaultConnection"];
            //migrationRunner.MigrationConnection(connStr);

            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Welcome}/{action=Login}/{id?}");
            });
        }
    }
}

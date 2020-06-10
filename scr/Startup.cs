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
using PortfolioWebApp.Data;
using PortfolioWebApp.Models.Accounts;
using PortfolioWebApp.Models.Settings.AppSettings;
using PortfolioWebApp.Services;
using PortfolioWebApp.Utils;

namespace PortfolioWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _environment = env;
        }

        private readonly IWebHostEnvironment _environment;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsDevelopment())
            {
                services.ConfigureWritable<AppSettings>(Configuration.GetSection("AppSettings"), "appsettings.Development.json");
            }
            else
            {
                services.ConfigureWritable<AppSettings>(Configuration.GetSection("AppSettings"), "appsettings.json");
            }


            services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>();
            services.AddRazorPages()
                .WithRazorPagesRoot("/Portfolio/Pages")
                .AddRazorPagesOptions(options => 
                {
                    options.Conventions.AuthorizeAreaFolder("Admin", "/");   
                }
                ).AddRazorRuntimeCompilation();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });


            

            services.AddTransient<SimplePageService>();
            services.AddTransient<PortfolioNavigationService>();
            services.AddTransient<SetupService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();              
            });
        }
    }
}

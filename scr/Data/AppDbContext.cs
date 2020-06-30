using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Models;
using PortfolioWebApp.Models.Accounts;
using PortfolioWebApp.Models.MyProjects;
using PortfolioWebApp.Models.Navigation;
using PortfolioWebApp.Models.SimplePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> Accounts { get; set; }
        public DbSet<SimplePage> SimplePages { get; set; }
        public DbSet<PortfolioNavigation> PortfolioNavigations { get; set; }
        public DbSet<MyProject> MyProjects { get; set; }
        public DbSet<MyProjectImage> MyProjectImages { get; set; }

    }
}

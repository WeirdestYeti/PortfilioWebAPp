using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using PortfolioWebApp.Data;
using PortfolioWebApp.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace PortfolioWebApp.Services
{
    public class SetupService
    {
        private readonly ILogger<SetupService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public SetupService(ILogger<SetupService> logger, UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<(bool, List<string>)> CreateDbAndAccountAsync(string connectionString, string email, string password)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>();

            options.UseMySql(connectionString);

            (bool, List<string>) result = (false, new List<string>());

            using(var db = new AppDbContext(options.Options))
            {
                bool dbConnectResponse = false;

                try
                {
                    dbConnectResponse = await db.Database.CanConnectAsync();
                }
                catch
                {                   
                    result.Item1 = false;
                }

                if (!dbConnectResponse)
                {
                    result.Item1 = false;
                    result.Item2.Add("Can't coonect to the database!");
                }
                else
                {
                    db.Database.Migrate();

                    ApplicationUser user = new ApplicationUser { UserName = email, Email = email };

                    var accountResult = await _userManager.CreateAsync(user, password);

                    if (accountResult.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        result.Item1 = true;
                        result.Item2.Add("Account successfully created.");
                    }
                    else
                    {
                        foreach (var error in accountResult.Errors)
                        {
                            result.Item2.Add(error.Description);
                        }

                        result.Item1 = false;
                    }

                }

            }

            return result;
        }

        public async Task<(bool, string)> TestDatabaseConnectionAsync(string dbConnectionString)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>();

            options.UseMySql(dbConnectionString);

            (bool, string) result = (true, "Connection established");

            using (var db = new AppDbContext(options.Options))
            {
                try
                {
                    bool response = await db.Database.CanConnectAsync();

                    if (!response)
                    {
                        result.Item1 = false;
                        result.Item2 = "Can't coonect to the database!";
                    }
                }
                catch
                {
                    result.Item2 = "Can't coonect to the database!";
                    result.Item1 = false;
                }
            }

            return result;
        }
    }
}

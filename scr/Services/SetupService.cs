using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using PortfolioWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace PortfolioWebApp.Services
{
    public class SetupService
    {
        public async Task<(bool, string)> CreateDatabaseAsync(string dbConnectionString)
        {
            (bool, string) result = await testDbConnectionAndEnsureCreatedAsync(dbConnectionString);


            return (result.Item1, result.Item2);
        }

        private async Task<(bool, string)> testDbConnectionAndEnsureCreatedAsync(string dbConnectionString)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>();

            options.UseMySql(dbConnectionString);

            (bool, string) result = (true, "Connection established");

            using (var db = new AppDbContext(options.Options))
            {

                try
                {
                    await db.Database.OpenConnectionAsync();
                    await db.Database.CloseConnectionAsync();

                    db.Database.EnsureCreated();
                }
                catch (MySqlException e)
                {
                    result.Item2 = e.Message;
                    result.Item1 = false;
                }
                result.Item1 = true;
            }

            return result;
        }
    }
}

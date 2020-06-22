using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Data;
using PortfolioWebApp.Models.MyProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Services
{
    public class MyProjectService
    {
        private readonly AppDbContext _dbContext;

        public MyProjectService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<List<MyProject>> GetAllAsync()
        {
            return await _dbContext.MyProjects.ToListAsync();
        }


        public async Task<(bool, string)> CreateAsync(MyProject myProject)
        {
            if(myProject != null)
            {
                _dbContext.MyProjects.Add(myProject);

                await _dbContext.SaveChangesAsync();

                return (true, "Project added successfully");
            }

            return (false, "Null object provided.");
        }
    }
}

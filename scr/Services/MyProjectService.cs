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

        public async Task<List<MyProject>> GetMyProjectsAsync()
        {
            return await _dbContext.MyProjects.ToListAsync();
        }
    }
}

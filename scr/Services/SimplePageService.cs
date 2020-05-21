using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Data;
using PortfolioWebApp.Models.SimplePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Services
{
    public class SimplePageService
    {
        private readonly AppDbContext _dbContext;
        public SimplePageService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<(bool, string)> AddNewPageAsync(SimplePage simplePage)
        {
            if (!string.IsNullOrEmpty(simplePage.CustomUrl))
            {
                if (!(await _dbContext.SimplePages.SingleOrDefaultAsync(x => x.CustomUrl.Equals(simplePage.CustomUrl)) == null))
                {
                    return (false, "Custom Url already exists.");
                }
                
            }

            _dbContext.SimplePages.Add(simplePage);
            await _dbContext.SaveChangesAsync();

            return (true, "Page successfully added.");
        }
    }
}

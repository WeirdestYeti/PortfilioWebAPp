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

        /// <summary>
        /// Adds a new simple page.
        /// </summary>
        /// <param name="simplePage"></param>
        /// <returns>Tupple of bool and string.</returns>
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

        /// <summary>
        /// Gets all pages.
        /// </summary>
        /// <returns>List of SimplePage</returns>
        public async Task<List<SimplePage>> GetAllPagesAsync()
        {
            return await _dbContext.SimplePages.ToListAsync();
        }
        
        /// <summary>
        /// Gets page by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return page or null if not found.</returns>
        public async Task<SimplePage> GetPageByIdAsync(int id)
        {
            return await _dbContext.SimplePages.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Updates page.
        /// </summary>
        /// <param name="simplePage"></param>
        /// <returns>Tupple of bool and string.</returns>
        public async Task<(bool, string)> UpdatePageAsync(SimplePage simplePage)
        {
            SimplePage simplePageDb = await _dbContext.SimplePages.SingleOrDefaultAsync(x => x.Id == simplePage.Id);
            if(simplePage != null)
            {
                if (!string.IsNullOrEmpty(simplePage.CustomUrl))
                {
                    if (!(await _dbContext.SimplePages.SingleOrDefaultAsync(x => (x.CustomUrl.Equals(simplePage.CustomUrl) && x.Id != simplePage.Id)) == null))
                    {
                        return (false, "Custom Url already exists.");
                    }
                }

                simplePageDb.Title = simplePage.Title;
                simplePageDb.CustomUrl = simplePage.CustomUrl;
                simplePageDb.HTML = simplePage.HTML;

                _dbContext.SimplePages.Update(simplePageDb);
                await _dbContext.SaveChangesAsync();

                return (true, "Page updated successfully.");
            }
            return (false, "Page with the Id: " + simplePage.Id + " not found.");
        }
    }
}

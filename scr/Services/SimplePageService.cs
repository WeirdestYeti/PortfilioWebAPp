using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Data;
using PortfolioWebApp.Models.SimplePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            SimplePage homePage = await _dbContext.SimplePages.SingleOrDefaultAsync(x => x.Title.Equals(simplePage.Title));

            if(homePage != null)
            {
                return (false, "There can only be one page with a title Home.");
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
        /// Gets page by Title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<SimplePage> GetPageByTitleAsync(string title)
        {
            return await _dbContext.SimplePages.SingleOrDefaultAsync(x => x.Title.Equals(title));
        }

        /// <summary>
        /// Updates page.
        /// </summary>
        /// <param name="simplePage"></param>
        /// <returns>Tupple of bool and string.</returns>
        public async Task<(bool, string)> UpdatePageAsync(SimplePage simplePage)
        {
            if (simplePage == null) return (false, "Null object reference provided.");
            SimplePage simplePageDb = await _dbContext.SimplePages.SingleOrDefaultAsync(x => x.Id == simplePage.Id);
            if(simplePageDb != null)
            {
                if (!string.IsNullOrEmpty(simplePage.CustomUrl))
                {
                    if (!(await _dbContext.SimplePages.SingleOrDefaultAsync(x => (x.CustomUrl.Equals(simplePage.CustomUrl) && x.Id != simplePage.Id)) == null))
                    {
                        return (false, "Custom Url already exists.");
                    }
                }

                if (simplePage.Title.Equals("Home") && !simplePageDb.Title.Equals("Home"))
                {
                    SimplePage homePage = await _dbContext.SimplePages.SingleOrDefaultAsync(x => x.Title.Equals(simplePage.Title));

                    if (homePage != null)
                    {
                        return (false, "There can only be one page with a title Home.");
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

        /// <summary>
        /// Deletes page by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Tupple of bool and string.</returns>
        public async Task<(bool, string)> DeletePageByIdAsync(int id)
        {
            SimplePage simplePage = await _dbContext.SimplePages.SingleOrDefaultAsync(x => x.Id == id);
            if(simplePage != null)
            {
                _dbContext.SimplePages.Remove(simplePage);
                await _dbContext.SaveChangesAsync();

                return (true, "Page deleted successfully");
            }
            return (false, "Page with the Id: " + id + " not found.");
        }

        /// <summary>
        /// Get page by custom url.
        /// </summary>
        /// <param name="customUrl"></param>
        /// <returns></returns>
        public async Task<SimplePage> GetPageByCustomUrl(string customUrl)
        {
            if(customUrl != null)
            {
                return await _dbContext.SimplePages.SingleOrDefaultAsync(x => x.CustomUrl.Equals(customUrl));
            }
            return null;
        }


        public async Task<SimplePage> GetPageByUrl(string url)
        {
            if(url != null)
            {
                string[] splitUrl = url.Split('-', 2);

                if(splitUrl.Length == 2)
                {
                    int id;

                    bool idParsedResult = int.TryParse(splitUrl[0], out id);

                    if (idParsedResult)
                    {
                        SimplePage simplePage = await _dbContext.SimplePages.SingleOrDefaultAsync(x => x.Id == id && string.Compare(splitUrl[1], x.Title) == 0);

                        return simplePage;
                    }                   
                }               
            }
            return null;
        }
    }
}
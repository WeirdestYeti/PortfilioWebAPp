using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Data;
using PortfolioWebApp.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PortfolioWebApp.Services
{
    public class PortfolioNavigationService
    {
        private readonly AppDbContext _dbContext;
        public PortfolioNavigationService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        /// <summary>
        /// Adds navigation item.
        /// </summary>
        /// <param name="navigation"></param>
        /// <returns>Tuple of bool and string.</returns>
        public async Task<(bool, string)> AddNavigationAsync(PortfolioNavigation navigation)
        {
            if(navigation != null)
            {
                int order = _dbContext.PortfolioNavigations.Count();
                navigation.Order = order + 1;

                _dbContext.PortfolioNavigations.Add(navigation);
                await _dbContext.SaveChangesAsync();

                return (true, "Page added successfully");
            }
            return (false, "Null object provided.");
        }

        /// <summary>
        /// Gets navigation in correct order.
        /// </summary>
        /// <returns>Ordered list of PortfolioNavigation</returns>
        public async Task<List<PortfolioNavigation>> GetNavigationsAsync()
        {
            List<PortfolioNavigation> navigations = await _dbContext.PortfolioNavigations.OrderBy(x => x.Order).ToListAsync();

            return navigations;
        }

        /// <summary>
        /// Updates navigation item.
        /// </summary>
        /// <param name="navigation"></param>
        /// <returns>Tuple of bool and string.</returns>
        public async Task<(bool, string)> UpdateNavigationAsync(PortfolioNavigation navigation)
        {
            if(navigation != null)
            {
                PortfolioNavigation dbNavigation = await _dbContext.PortfolioNavigations.SingleOrDefaultAsync(x => x.Id == navigation.Id);

                if(dbNavigation != null)
                {
                    dbNavigation.Name = navigation.Name;
                    dbNavigation.Url = navigation.Url;

                    _dbContext.PortfolioNavigations.Update(dbNavigation);
                    await _dbContext.SaveChangesAsync();

                    return (true, "Navigation item updated successfully.");
                }

                return (false, "Item not found.");
            }
            return (false, "Null object provided.");
        }

        /// <summary>
        /// Gets Navigation by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PortfolioNavigation> GetNavigationByIdAsync(int id)
        {
            return await _dbContext.PortfolioNavigations.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(bool, string)> DeleteNavigationByIdAsync(int id)
        {
            if(id != 0)
            {
                PortfolioNavigation navigation = await _dbContext.PortfolioNavigations.SingleOrDefaultAsync(x => x.Id == id);
                
                if(navigation != null)
                {
                    List<PortfolioNavigation> navigationsList = await _dbContext.PortfolioNavigations.Where(x => x.Order > navigation.Order).ToListAsync();
                    for (int i = 0; i < navigationsList.Count; i++)
                    {
                        navigationsList[i].Order = navigationsList[i].Order - 1;
                        _dbContext.PortfolioNavigations.Update(navigationsList[i]);
                    }

                    _dbContext.PortfolioNavigations.Remove(navigation);
                    await _dbContext.SaveChangesAsync();

                    return (true, "Navigation item deleted successfully.");
                }    
            }
            return (false, "Item not found.");
        }

    }
}

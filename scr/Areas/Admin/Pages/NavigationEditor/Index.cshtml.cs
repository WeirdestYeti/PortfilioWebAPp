using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWebApp.Models.Navigation;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Areas.Admin.Pages.NavigationEditor
{
    public class IndexModel : PageModel
    {
        private readonly PortfolioNavigationService _navigationService;
        public IndexModel(PortfolioNavigationService portfolioNavigationService)
        {
            _navigationService = portfolioNavigationService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public List<PortfolioNavigation> Navigations { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Navigations = await _navigationService.GetNavigationsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostMoveNaviDownAsync(int id)
        {
            if(id != 0)
            {
                (bool, string) result = await _navigationService.MoveNavigationAsync(id, MoveNavi.Down);

                StatusMessage = result.Item2;
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMoveNaviUpAsync(int id)
        {
            if(id != 0)
            {
                (bool, string) result = await _navigationService.MoveNavigationAsync(id, MoveNavi.Up);

                StatusMessage = result.Item2;
            }
            return RedirectToPage();
        }
    }
}
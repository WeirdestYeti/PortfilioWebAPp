using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using PortfolioWebApp.Models.Navigation;
using PortfolioWebApp.Models.Settings.AppSettings;
using PortfolioWebApp.Models.SimplePages;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Areas.Portfolio.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SimplePageService _simplePageService;

        public IndexModel(SimplePageService simplePageService)
        {
            _simplePageService = simplePageService;
        }

        public SimplePage SimplePage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SimplePage = await _simplePageService.GetPageByTitleAsync("Home");

            return Page();
        }
    }
}
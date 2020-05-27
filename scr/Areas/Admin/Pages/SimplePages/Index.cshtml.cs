using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWebApp.Models.SimplePages;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Areas.Admin.Pages.SimplePages
{
    public class IndexModel : PageModel
    {
        private readonly SimplePageService _simplePageService;
        
        public IndexModel(SimplePageService simplePageService)
        {
            _simplePageService = simplePageService;
        }

        public bool ShowHomePageInfo { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public List<SimplePage> SimplePages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SimplePages = await _simplePageService.GetAllPagesAsync();

            SimplePage homePage = SimplePages.SingleOrDefault(x => x.Title.Equals("Home"));

            if (homePage == null) ShowHomePageInfo = true;
            else ShowHomePageInfo = false;

            return Page();
        }

    }
}
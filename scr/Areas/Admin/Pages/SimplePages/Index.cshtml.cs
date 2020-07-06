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

        public bool IsHomePageSetUp { get; set; } = false;
        public bool IsContactPageSetUp { get; set; } = false;

        [TempData]
        public string StatusMessage { get; set; }

        public List<SimplePage> SimplePages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SimplePages = await _simplePageService.GetAllPagesAsync();

            if(SimplePages.Any(x => x.Title.Equals("Home")))
            {
                IsHomePageSetUp = true;
            }

            if(SimplePages.Any(x => x.Title.Equals("Contact")))
            {
                IsContactPageSetUp = true;
            }

            return Page();
        }

    }
}
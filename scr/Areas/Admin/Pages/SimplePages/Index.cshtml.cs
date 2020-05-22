using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<SimplePage> SimplePages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SimplePages = await _simplePageService.GetAllPagesAsync();

            return Page();
        }

    }
}
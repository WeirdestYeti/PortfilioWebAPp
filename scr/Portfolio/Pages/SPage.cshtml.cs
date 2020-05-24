using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWebApp.Models.SimplePages;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Portfolio.Pages
{
    public class SPageModel : PageModel
    {
        private readonly SimplePageService _simplePageService;

        public SPageModel(SimplePageService simplePageService)
        {
            _simplePageService = simplePageService;
        }

        public SimplePage SimplePage { get; set; }

        public async Task<IActionResult> OnGetAsync(string url)
        {
            // First we check for custorm url.
            SimplePage = await _simplePageService.GetPageByCustomUrl(url);
            if(SimplePage == null)
            {
                SimplePage = await _simplePageService.GetPageByUrl(url);
                if(SimplePage != null)
                {

                    return Page();
                }
                return LocalRedirect("/errors/404");
            }
            return Page();
        }
    }
}
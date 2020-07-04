using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWebApp.Models.MyProjects;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Portfolio.Pages.MyProjects
{
    public class ViewModel : PageModel
    {
        private readonly MyProjectService _projectService;

        public ViewModel(MyProjectService projectService)
        {
            _projectService = projectService;
        }

        public MyProject MyProject { get; set; }

        public async Task<IActionResult> OnGetAsync(string url)
        {
            if(url != null)
            {
                MyProject = await _projectService.GetByUrlWithImagesAsync(url);
                if(MyProject != null)
                {

                    return Page();
                }
            }

            return LocalRedirect("/MyProjects");
        }
    }
}
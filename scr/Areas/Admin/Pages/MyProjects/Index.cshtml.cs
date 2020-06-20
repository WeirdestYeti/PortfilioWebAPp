using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWebApp.Models.MyProjects;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Areas.Admin.Pages.MyProjects
{
    public class IndexModel : PageModel
    {
        private readonly MyProjectService _myProjectService;

        public IndexModel(MyProjectService myProjectService)
        {
            _myProjectService = myProjectService;
        }


        public List<MyProject> MyProjects { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            MyProjects = await _myProjectService.GetMyProjectsAsync();

            return Page();
        }
    }
}
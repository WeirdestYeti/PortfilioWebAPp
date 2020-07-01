using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWebApp.Models.MyProjects;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Areas.Admin.Pages.MyProjects
{
    public class DeleteModel : PageModel
    {
        public readonly MyProjectService _projectService;

        public DeleteModel(MyProjectService projectService)
        {
            _projectService = projectService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public string Title { get; set; }
        public string ShortDescription { get; set; }

        [BindProperty]
        [Required]
        [HiddenInput]
        public int DeleteId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id != null)
            {
                MyProject myProject = await _projectService.GetByIdAsync((int)id);
                if (myProject != null)
                {

                    DeleteId = myProject.Id;
                    Title = myProject.Title;
                    ShortDescription = myProject.ShortDescription;

                    return Page();
                }
            }

            return LocalRedirect("/Admin/MyProjects");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (DeleteId != 0)
                {
                    (bool, string) result = await _projectService.DeleteMyProjectByIdAsync(DeleteId);

                    if (result.Item1)
                    {
                        StatusMessage = result.Item2;
                        return LocalRedirect("/Admin/MyProjects");
                    }
                    else
                    {
                        StatusMessage = "Error: " + result.Item2;
                        return LocalRedirect("/Admin/MyProjects");
                    }
                }
            }

            return LocalRedirect("/Admin/MyProjects");
        }
    }
}
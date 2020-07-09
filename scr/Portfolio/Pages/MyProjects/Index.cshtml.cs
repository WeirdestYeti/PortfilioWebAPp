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
    public class IndexModel : PageModel
    {
        private readonly MyProjectService _projectService;

        public IndexModel(MyProjectService projectService)
        {
            _projectService = projectService;
        }

        public class Project
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string ShortDescription { get; set; }
            public string ThumbnailUrl { get; set; }
            public bool IsRepositoryPrivate { get; set; }
        }

        public List<Project> Projects { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            List<MyProject> MyProjects = await _projectService.GetAllAsync();

            Projects = new List<Project>();

            for (int i = 0; i < MyProjects.Count; i++)
            {
                Projects.Add(
                    new Project()
                    {
                        Id = MyProjects[i].Id,
                        Title = MyProjects[i].Title,
                        ShortDescription = MyProjects[i].ShortDescription,
                        ThumbnailUrl = MyProjects[i].ThumbnailUrl,
                        IsRepositoryPrivate = MyProjects[i].IsRepositoryPrivate
                    }
                );
            }

            if(Projects != null)
            {
                Projects = Projects.OrderByDescending(x => x.Id).ToList();
            }
            

            return Page();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;
using PortfolioWebApp.Models.MyProjects;
using PortfolioWebApp.Services;
using PortfolioWebApp.Utils;

namespace PortfolioWebApp.Areas.Admin.Pages.MyProjects
{
    public class AddModel : PageModel
    {
        private readonly MyProjectService _projectService;

        private string[] permittedExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

        public AddModel(IWebHostEnvironment environment, MyProjectService projectService)
        {
            _projectService = projectService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [MaxLength(60)]
            public string Title { get; set; }
            [Required]
            [MaxLength(256)]
            public string ShortDescription { get; set; }
            public bool IsRepositoryPrivate { get; set; }
            [MaxLength(256)]
            public string RepositoryUrl { get; set; }
            [MaxLength(256)]
            [Display(Name = "Thumbnail Url")]
            public string ThumbnailUrl { get; set; }
            public bool ShowSlideshow { get; set; }
            public string HTMLContent { get; set; }
            public List<IFormFile> Images { get; set; }
        }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Input.Images != null && Input.Images.Count > 0)
                {
                    bool areFilesAllowed = true;
                    foreach (var image in Input.Images)
                    {
                        var ext = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                        if (!permittedExtensions.Contains(ext.ToLower()))
                        {
                            areFilesAllowed = false;
                        }
                    }

                    if (!areFilesAllowed)
                    {
                        StatusMessage = "Error: Please specify a valid image file (.jpg, .jpeg, .gif or .png)";

                        return Page();
                    }
                }

                MyProject myProject = new MyProject();

                myProject.IsRepositoryPrivate = Input.IsRepositoryPrivate;
                myProject.RepositoryUrl = Input.RepositoryUrl;
                myProject.Title = Input.Title;
                myProject.ShortDescription = Input.ShortDescription;
                myProject.HTMLContent = Input.HTMLContent;
                myProject.ShowSlideshow = Input.ShowSlideshow;
                myProject.ThumbnailUrl = Input.ThumbnailUrl;

                (bool, string) result = await _projectService.CreateAsync(myProject, Input.Images);

                StatusMessage = result.Item2;

                if (result.Item1)
                {
                    StatusMessage = result.Item2;
                    return RedirectToPage("Index");
                }
                else
                {
                    StatusMessage = "Error: " + result.Item2;
                }

            }

            return Page();
        }
    }
}
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
        private IWebHostEnvironment _environment;

        private string[] permittedExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

        public AddModel(IWebHostEnvironment environment, MyProjectService projectService)
        {
            _environment = environment;
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
                List<MyProjectImage> images = new List<MyProjectImage>();

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

                    var serverPath = Path.Combine(_environment.WebRootPath);
                    var imagePath = "uploads/images/" + "monthly_" + DateTime.Now.ToString("yyyy_MM");

                    var fullPath = Path.Combine(serverPath, imagePath);

                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }

                    for (int i = 0; i < Input.Images.Count; i++)
                    {
                        string ext = Input.Images[i].FileName.Substring(Input.Images[i].FileName.LastIndexOf('.'));
                        string fileName = DateTimeOffset.Now.ToUnixTimeSeconds() + "_"  + RandomString.GenerateRandomString() + ext;

                        var file = Path.Combine(fullPath, fileName);

                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            await Input.Images[i].CopyToAsync(fileStream);
                        }

                        images.Add(new MyProjectImage()
                        {
                            Time = DateTime.Now,
                            Location = Path.Combine(imagePath, fileName)
                        });
                    }
                }

                MyProject myProject = new MyProject();

                myProject.IsRepositoryPrivate = Input.IsRepositoryPrivate;
                myProject.RepositoryUrl = Input.RepositoryUrl;
                myProject.Title = Input.Title;
                myProject.ShortDescription = Input.ShortDescription;
                myProject.HTMLContent = Input.HTMLContent;
                myProject.MyProjectImages = images;

                (bool, string) result = await _projectService.CreateAsync(myProject);

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
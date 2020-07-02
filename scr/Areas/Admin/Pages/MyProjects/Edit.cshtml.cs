using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PortfolioWebApp.Models.MyProjects;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Areas.Admin.Pages.MyProjects
{
    public class EditModel : PageModel
    {
        private readonly MyProjectService _projectService;


        private string[] permittedExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

        public EditModel(MyProjectService projectService)
        {
            _projectService = projectService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<MyProjectImage> MyProjectImages { get; set; }
        public class InputModel
        {
            [Required]
            [HiddenInput]
            public int Id { get; set; }
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
            public bool ShowSlideshow { get; set; }
            public List<ExistingImagesEdit> ExistingImagesEdit { get; set; }

            public List<IFormFile> NewImages { get; set; }
        }

        public class ExistingImagesEdit
        {
            [Required]
            [HiddenInput]
            public int Id { get; set; }
            public bool SelectedToDelete { get; set; }

        }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id != null)
            {
                MyProject myProject = await _projectService.GetByIdAsync((int)id);
                if(myProject != null)
                {
                    Input = new InputModel();

                    Input.Id = myProject.Id;
                    Input.Title = myProject.Title;
                    Input.ShortDescription = myProject.ShortDescription;
                    Input.IsRepositoryPrivate = myProject.IsRepositoryPrivate;
                    Input.RepositoryUrl = myProject.RepositoryUrl;
                    Input.HTMLContent = myProject.HTMLContent;
                    Input.ShowSlideshow = myProject.ShowSlideshow;

                    MyProjectImages = myProject.MyProjectImages;

                    if(MyProjectImages != null)
                    {
                        Input.ExistingImagesEdit = new List<ExistingImagesEdit>();
                        foreach (var image in MyProjectImages)
                        {
                            Input.ExistingImagesEdit.Add(new ExistingImagesEdit()
                            {
                                Id = image.Id,
                                SelectedToDelete = false
                            });
                        }
                    }

                    return Page();
                }
                StatusMessage = "Error: Item not found.";
                return LocalRedirect("/Admin/MyProjects");
            }
            StatusMessage = "Error: Item not found.";
            return LocalRedirect("/Admin/MyProjects");
        }

        private async Task GetProjectImages(int projectId)
        {
            MyProjectImages = await _projectService.GetProjectImagesByIdAsync(projectId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Input.NewImages != null && Input.NewImages.Count > 0)
                {
                    bool areFilesAllowed = true;
                    foreach (var image in Input.NewImages)
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

                        await GetProjectImages(Input.Id);
                        return Page();
                    }
                }

                for (int i = 0; i < Input.ExistingImagesEdit.Count; i++)
                {
                    if (Input.ExistingImagesEdit[i].SelectedToDelete)
                    {
                        await _projectService.DeleteProjectImageByIdAsync(Input.Id, Input.ExistingImagesEdit[i].Id);
                    }
                }

                MyProject myProject = new MyProject();

                myProject.Id = Input.Id;
                myProject.Title = Input.Title;
                myProject.ShortDescription = Input.ShortDescription;
                myProject.IsRepositoryPrivate = Input.IsRepositoryPrivate;
                myProject.RepositoryUrl = Input.RepositoryUrl;
                myProject.HTMLContent = Input.HTMLContent;

                (bool, string) result = await _projectService.EditAsync(myProject, Input.NewImages);

                if (!result.Item1)
                {
                    StatusMessage = "Error: " + result.Item2;
                }
                else
                {
                    StatusMessage = result.Item2;
                }

                // Redirec to get so the picture data would be loaded again correcrtly after the deletion.
                return LocalRedirect("/Admin/MyProjects/Edit/" + myProject.Id);
            }

            await GetProjectImages(Input.Id);
            return Page();
        }
    }
}
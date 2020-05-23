using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PortfolioWebApp.Models.SimplePages;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Areas.Admin.Pages.SimplePages
{
    public class AddModel : PageModel
    {
        private readonly SimplePageService _simplePageService;

        public AddModel(SimplePageService simplePageService)
        {
            _simplePageService = simplePageService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {

        }

        public class InputModel
        {
            [Required]
            [MaxLength(60)]
            public string Title { get; set; }
            [MaxLength(60)]
            [DisplayName("Custorm Url")]
            public string CustomUrl { get; set; }
            public string HTML { get; set; }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                SimplePage simplePage = new SimplePage();

                if (!string.IsNullOrEmpty(Input.CustomUrl) || !string.IsNullOrWhiteSpace(Input.CustomUrl))
                {
                    if (Input.CustomUrl.Contains(" "))
                    {
                        ModelState.AddModelError(string.Empty, "Custom Url can't contain spaces.");
                        return Page();
                    }
                    simplePage.CustomUrl = Input.CustomUrl;
                }

                simplePage.Title = Input.Title;
                simplePage.HTML = Input.HTML;                       

                (bool, string) result = await _simplePageService.AddNewPageAsync(simplePage);

                if (!result.Item1)
                {
                    ModelState.AddModelError(string.Empty, result.Item2);
                    return Page();
                }
                else
                {
                    StatusMessage = result.Item2;
                    return LocalRedirect("/Admin/SimplePages");
                }
            }

            return Page();
        }
    }
}
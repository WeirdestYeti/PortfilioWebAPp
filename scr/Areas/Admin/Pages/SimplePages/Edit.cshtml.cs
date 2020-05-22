using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWebApp.Models.SimplePages;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Areas.Admin.Pages.SimplePages
{
    public class EditModel : PageModel
    {
        private readonly SimplePageService _simplePageService;

        public EditModel(SimplePageService simplePageService)
        {
            _simplePageService = simplePageService;            
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public int EditId { get; set; }

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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id != null)
            {
                SimplePage simplePage = await _simplePageService.GetPageByIdAsync((int)id);
                if (simplePage == null) return LocalRedirect("/Admin/SimplePages");

                Input = new InputModel();
                EditId = simplePage.Id;
                Input.Title = simplePage.Title;
                Input.CustomUrl = simplePage.CustomUrl;
                Input.HTML = simplePage.HTML;

                return Page();
            }
            return LocalRedirect("/Admin/SimplePages");
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

                simplePage.Id = EditId;
                simplePage.Title = Input.Title;
                simplePage.HTML = Input.HTML;

                (bool, string) result = await _simplePageService.UpdatePageAsync(simplePage);

                if (!result.Item1)
                {
                    ModelState.AddModelError(string.Empty, result.Item2);
                    return Page();
                }
                else
                {
                    return LocalRedirect("/Admin/SimplePages");
                }
            }

            return Page();
        }
    }
}
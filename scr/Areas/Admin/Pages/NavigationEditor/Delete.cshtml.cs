using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWebApp.Models.Navigation;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Areas.Admin.Pages.NavigationEditor
{
    public class DeleteModel : PageModel
    {
        private readonly PortfolioNavigationService _navigationService;
        public DeleteModel(PortfolioNavigationService portfolioNavigationService)
        {
            _navigationService = portfolioNavigationService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public PortfolioNavigation Navigation { get; set; }

        [BindProperty]
        [Required]
        [HiddenInput]
        public int DeleteId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id != null)
            {
                Navigation = await _navigationService.GetNavigationByIdAsync((int)id);
                if(Navigation != null)
                {
                    DeleteId = (int)id;
                    return Page();
                }
                return LocalRedirect("/Admin/NavigationEditor");
            }
            return LocalRedirect("/Admin/NavigationEditor");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if(DeleteId != 0)
                {
                    (bool, string) result = await _navigationService.DeleteNavigationByIdAsync(DeleteId);

                    if (result.Item1)
                    {
                        StatusMessage = result.Item2;
                        return LocalRedirect("/Admin/NavigationEditor");
                    }
                    else
                    {
                        StatusMessage = "Error: " + result.Item2;
                        return Page();
                    }
                }
            }
            return LocalRedirect("/Admin/NavigationEditor");
        }
    }
}
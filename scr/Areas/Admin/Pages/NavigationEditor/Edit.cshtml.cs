using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PortfolioWebApp.Models.Navigation;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Areas.Admin.Pages.NavigationEditor
{
    public class EditModel : PageModel
    {
        private readonly PortfolioNavigationService _navigationService;

        public EditModel(PortfolioNavigationService portfolioNavigationService)
        {
            _navigationService = portfolioNavigationService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [HiddenInput]
            public int Id { get; set; }
            [Required]
            [MaxLength(32)]
            [MinLength(2)]
            public string Name { get; set; }
            [Required]
            [MinLength(1)]
            [MaxLength(250)]
            public string Url { get; set; }
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id != null)
            {
                PortfolioNavigation navigation = await _navigationService.GetNavigationByIdAsync((int)id);
                if(navigation != null)
                {
                    Input = new InputModel();
                    Input.Id = navigation.Id;
                    Input.Name = navigation.Name;
                    Input.Url = navigation.Url;

                    return Page();
                }

                StatusMessage = "Error: Item not found";
                return LocalRedirect("/Admin/NavigationEditor");
            }

            StatusMessage = "Error: Item not found";
            return LocalRedirect("/Admin/NavigationEditor");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                PortfolioNavigation navigation = new PortfolioNavigation();

                navigation.Id = Input.Id;

                navigation.Name = Input.Name;
                navigation.Url = Input.Url;

                (bool, string) result = await _navigationService.UpdateNavigationAsync(navigation);

                if (result.Item1)
                {
                    StatusMessage = result.Item2;
                    return Page();
                }
                else
                {
                    StatusMessage = "Error:" + result.Item2;
                    return Page();
                }

            }

            return Page();
        }
    }
}
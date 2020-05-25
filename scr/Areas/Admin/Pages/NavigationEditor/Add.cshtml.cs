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
    public class AddModel : PageModel
    {
        private readonly PortfolioNavigationService _navigationService;

        public AddModel(PortfolioNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public string ErrorMessage { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [MaxLength(32)]
            [MinLength(2)]
            public string Name { get; set; }
            [Required]
            [MinLength(1)]
            [MaxLength(250)]
            public string Url { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                PortfolioNavigation navigation = new PortfolioNavigation();

                navigation.Name = Input.Name;
                navigation.Url = Input.Url;

                (bool, string) result = await _navigationService.AddNavigationAsync(navigation);

                if (result.Item1)
                {
                    StatusMessage = result.Item2;
                    return LocalRedirect("/Admin/NavigationEditor");
                }
                else
                {
                    ErrorMessage = "Error:" + result.Item2;
                }
            }
            return Page();
        }
    }
}
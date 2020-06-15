using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using PortfolioWebApp.Models.Settings.AppSettings;
using PortfolioWebApp.Services;
using PortfolioWebApp.Utils.WritableOptions;

namespace PortfolioWebApp.Areas.Setup.Pages
{

    

    public class IndexModel : PageModel
    {
        public enum SetupStep
        {
            Database,
            Mailer,
            Account,
            Other,
            Finished
        }

        private IWritableOptions<AppSettingsOptions> _appSettings;

        private readonly SetupService _setupService;

        public bool IsSetupFinished { get; set; }

        public SetupStep CurrentSetupStep { get; set; }

        public IndexModel(IWritableOptions<AppSettingsOptions> appSettings, SetupService setupService)
        {
            _setupService = setupService;
            _appSettings = appSettings;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            // Database

            [Required]
            [Display(Name = "Server")]
            public string DbServer { get; set; }
            [Required]
            [Display(Name = "Database")]
            public string Db{ get; set; }
            [Required]
            [Display(Name = "Username")]
            public string DbUser { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string DbPassword { get; set; }

            // Mailing

            [Display(Name = "Host")]
            public string MailerHost { get; set; }
            [Display(Name = "Port")]
            public int MailerPort { get; set; }
            [Display(Name = "Username")]
            public string MailerCredentialUserName { get; set; }
            [Display(Name = "Password")]
            public string MailerCredentialPassword { get; set; }

            // Account

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            // Other

            [Required]
            [Display(Name = "Portfolio Title")]
            public string PortfolioTitle { get; set; }


        }

        public IActionResult OnGet()
        {
            IsSetupFinished = _appSettings.Value.SetupFinished;

            if (IsSetupFinished)
            {
                return LocalRedirect("/");
            }

            CurrentSetupStep = SetupStep.Database;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // For step-by-step validation to work we need to remove validaton from ModelState for all next steps.

            if (CurrentSetupStep == SetupStep.Database)
            {
                // Removing next step validation.
                ModelState.Remove("Input.Email");
                ModelState.Remove("Input.Password");
                ModelState.Remove("Input.PortfolioTitle");

                if (ModelState.IsValid)
                {
                    (bool, string) result = await _setupService.TestDatabaseConnectionAsync($"Server={Input.DbServer};Database={Input.Db};User={Input.DbUser};Password={Input.DbPassword};");

                    if (!result.Item1)
                    {
                        StatusMessage = "Error: " + result.Item2;
                    }
                    else
                    {

                        CurrentSetupStep = SetupStep.Mailer;

                        return Page();
                    }
                }
            }
            else if (CurrentSetupStep == SetupStep.Mailer)
            {
                // Removing next step validation.
                ModelState.Remove("Input.PortfolioTitle");


                if (ModelState.IsValid)
                {

                    CurrentSetupStep = SetupStep.Other;

                    return Page();
                }
            }            
            else if (CurrentSetupStep == SetupStep.Other)
            {
                if (ModelState.IsValid)
                {

                    CurrentSetupStep = SetupStep.Account;

                    return Page();           
                }
            }
            else if (CurrentSetupStep == SetupStep.Account)
            {
                if (ModelState.IsValid)
                {


                    // finish and save everything here

                    return Page();
                }

            }
            else
            {

            }

            return Page();
        }
    }
}
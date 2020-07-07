using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWebApp.Models.Settings;
using PortfolioWebApp.Models.Settings.AppSettings;
using PortfolioWebApp.Utils.WritableOptions;

namespace PortfolioWebApp.Areas.Admin.Pages
{
    public class SettingsModel : PageModel
    {
        public IWritableOptions<AppSettingsOptions> _appSettings;
        public IWritableOptions<MailerSettingsOptions> _mailerSettings;

        public SettingsModel(IWritableOptions<AppSettingsOptions> appSettings, IWritableOptions<MailerSettingsOptions> mailerSettings)
        {
            _appSettings = appSettings;
            _mailerSettings = mailerSettings;
        }

        public class AppSettingsInputModel
        {
            [Required]
            public string AppTitle { get; set; }
            public string ConnectionString { get; set; }
        }

        public class MailerSettingsInputModel
        {
            [Required]
            [EmailAddress]
            public string EmailFrom { get; set; }
            [Required]
            public string Host { get; set; }
            [Required]
            public int Port { get; set; }
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }
        }
     
        public AppSettingsInputModel AppSettingsInput { get; set; }
        public MailerSettingsInputModel MailerSettingsInput { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        private void GetInputData()
        {
            AppSettingsInput = new AppSettingsInputModel
            {
                AppTitle = _appSettings.Value.PortfolioTitle,
                ConnectionString = _appSettings.Value.ConnectionString
            };

            MailerSettingsInput = new MailerSettingsInputModel
            {
                EmailFrom = _mailerSettings.Value.EmailFrom,
                Host = _mailerSettings.Value.Host,
                Port = _mailerSettings.Value.Port,
                Username = _mailerSettings.Value.CredentialUserName,
                Password = _mailerSettings.Value.CredentialPassword
            };
        }

        public void OnGet()
        {
            GetInputData();
        }


        public IActionResult OnPostAppSettings(AppSettingsInputModel appSettingsInput)
        {

            if (ModelState.IsValid)
            {
                _appSettings.Update(opt =>
               {
                   opt.PortfolioTitle = appSettingsInput.AppTitle;
               });
            }
            GetInputData();
            return Page();
        }

        public IActionResult OnPostMailerSettings(MailerSettingsInputModel mailerSettingsInput)
        {
            if (ModelState.IsValid)
            {
                _mailerSettings.Update(opt =>
                {
                    opt.EmailFrom = mailerSettingsInput.EmailFrom;
                    opt.Host = mailerSettingsInput.Host;
                    opt.Port = mailerSettingsInput.Port;
                    opt.CredentialUserName = mailerSettingsInput.Username;
                    opt.CredentialPassword = mailerSettingsInput.Password;
                });

            }
            GetInputData();
            return Page();
        }
    }
}
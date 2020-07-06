using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using PortfolioWebApp.Models.Settings;
using PortfolioWebApp.Models.SimplePages;
using PortfolioWebApp.Services;

namespace PortfolioWebApp.Portfolio.Pages
{
    public class ContactModel : PageModel
    {
        private readonly SimplePageService _simplePageService;
        private readonly IOptionsSnapshot<MailerSettingsOptions> _mailerOptions;

        public ContactModel(SimplePageService simplePageService, IOptionsSnapshot<MailerSettingsOptions> mailerOptions)
        {
            _mailerOptions = mailerOptions;
            _simplePageService = simplePageService;
        }

        public SimplePage SimplePage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Name { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Subject { get; set; }
            [Required]
            public string Message { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            SimplePage = await _simplePageService.GetPageByTitleAsync("Contact");

            if(string.IsNullOrEmpty(_mailerOptions.Value.EmailFrom) || string.IsNullOrEmpty(_mailerOptions.Value.Host) || string.IsNullOrEmpty(_mailerOptions.Value.CredentialUserName) 
                || string.IsNullOrEmpty(_mailerOptions.Value.CredentialPassword))
            {
                StatusMessage = "Error: Mailer is not set up";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(Input.Email); //Form email
                    //from contact us page 
                    msz.To.Add(_mailerOptions.Value.EmailFrom); // For contact page we are using email adress which are we sending from for other use cases.
                    msz.Subject = Input.Subject;
                    msz.Body = Input.Message;

                    SmtpClient smtpClient = new SmtpClient
                    {
                        Host = _mailerOptions.Value.Host,
                        Port = _mailerOptions.Value.Port,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential(_mailerOptions.Value.CredentialUserName, _mailerOptions.Value.CredentialPassword),
                        EnableSsl = true
                    };

                    smtpClient.Send(msz);

                    ModelState.Clear();
                    StatusMessage = "Thank you for Contacting me.";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    StatusMessage = "Error: Sorry we are facing Problem here -" + ex.Message;
                }
            }

            return Page();
        }
    }
}
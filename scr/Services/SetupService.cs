using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using PortfolioWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace PortfolioWebApp.Services
{
    public class SetupService
    {


        public SetupService()
        {

        }

        //public async Task<(bool, string)> CreateAccountAsync(string email, string password)
        //{

        //        var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
        //        var result = await _userManager.CreateAsync(user, Input.Password);
        //        if (result.Succeeded)
        //        {
        //            _logger.LogInformation("User created a new account with password.");

        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //            var callbackUrl = Url.Page(
        //                "/Account/ConfirmEmail",
        //                pageHandler: null,
        //                values: new { area = "Identity", userId = user.Id, code = code },
        //                protocol: Request.Scheme);

        //            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        //                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //            if (_userManager.Options.SignIn.RequireConfirmedAccount)
        //            {
        //                return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
        //            }
        //            else
        //            {
        //                await _signInManager.SignInAsync(user, isPersistent: false);
        //                return LocalRedirect(returnUrl);
        //            }
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //}

        public async Task<(bool, string)> TestDatabaseConnectionAsync(string dbConnectionString)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>();

            options.UseMySql(dbConnectionString);

            (bool, string) result = (true, "Connection established");

            using (var db = new AppDbContext(options.Options))
            {
                try
                {
                    bool response = await db.Database.CanConnectAsync();

                    if (!response)
                    {
                        result.Item1 = false;
                        result.Item2 = "Can't coonect to the database!";
                    }
                }
                catch
                {
                    result.Item2 = "Can't coonect to the database!";
                    result.Item1 = false;
                }
            }

            return result;
        }
    }
}

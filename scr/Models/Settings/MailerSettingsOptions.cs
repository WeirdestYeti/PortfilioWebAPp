using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Models.Settings
{
    public class MailerSettingsOptions
    {
        public const string MailerSettings = "MailerSettings";
        public string EmailFrom { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string CredentialUserName { get; set; }
        public string CredentialPassword { get; set; }
    }
}

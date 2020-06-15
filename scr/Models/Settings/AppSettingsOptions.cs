using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Models.Settings.AppSettings
{
    public class AppSettingsOptions
    {
        public const string AppSettings = "AppSettings";
        public string PortfolioTitle { get; set; }
        public bool SetupFinished { get; set; }
        public string ConnectionString { get; set; }
    }
}

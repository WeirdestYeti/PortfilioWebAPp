using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Models.Settings.AppSettings
{
    public class AppSettings
    {
        public string PortfolioTitle { get; set; }
        public SetupSteps CurrentSetupStep { get; set; }
    }

    public enum SetupSteps
    {
        Database,
        Account,
        Other,
        Finished
    }
}

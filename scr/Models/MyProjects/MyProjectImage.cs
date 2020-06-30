using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Models.MyProjects
{
    public class MyProjectImage
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        [MaxLength(200)]
        public string Location { get; set; }
        public MyProject MyProject { get; set; }
}
}

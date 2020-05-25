using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Models.Navigation
{
    public class PortfolioNavigation
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Url { get; set; }
        public int Order { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Models.SimplePages
{
    public class Page
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url 
        {
            get
            {
                return Id + "-" + Name;
            }
        }
    }
}

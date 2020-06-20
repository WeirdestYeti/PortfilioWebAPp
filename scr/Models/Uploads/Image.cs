using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Models.Uploads
{
    public class Image
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        [MaxLength(200)]
        public string Location { get; set; }
    }
}

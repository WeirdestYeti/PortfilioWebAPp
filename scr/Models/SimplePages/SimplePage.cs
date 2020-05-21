using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Models.SimplePages
{
    public class SimplePage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(60)]
        public string Title { get; set; }
        [MaxLength(60)]
        public string CustomUrl { get; set; }
        public string Url 
        {
            get
            {
                if(CustomUrl == null)
                {
                    return Id + "-" + Title;
                }
                else
                {
                    return CustomUrl;
                }
            }
        }

        public string HTML { get; set; }
    }
}

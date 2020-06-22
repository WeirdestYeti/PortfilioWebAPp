using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.Models.MyProjects
{
    public class MyProject
    {
        public int Id { get; set; }
        [MaxLength(60)]
        public string Title { get; set; }
        [MaxLength(256)]
        public string ShortDescription { get; set; }
        public bool IsRepositoryPrivate { get; set; }
        [MaxLength(256)]
        public string RepositoryUrl { get; set; }
        public string HTMLContent { get; set; }
        public List<MyProjectImage> MyProjectImages { get; set; }
    }
}

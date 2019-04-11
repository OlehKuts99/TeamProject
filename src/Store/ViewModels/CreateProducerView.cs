using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Store.ViewModels
{
    public class CreateProducerView
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "WebSite")]
        public string WebSite { get; set; }

        
    }
}

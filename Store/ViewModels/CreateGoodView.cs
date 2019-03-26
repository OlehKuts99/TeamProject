using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Store.Models;

namespace Store.ViewModels
{
    public class CreateGoodView
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Specification")]
        public string Specification { get; set; }

        [Required]
        [Display(Name = "Photo Url")]
        public string PhotoUrl { get; set; }

        [Required]
        [Display(Name = "Year of goods production")]
        public int YearOfManufacture { get; set; }

        [Required]
        [Display(Name = "The warranty period")]
        public int WarrantyTerm { get; set; }

        [Required]
        [Display(Name = "Producer id")]
        public int ProducerId { get; set; }

        [Required]
        [Display(Name = "Producer")]
        public Producer Producer { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Product type")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public int Count { get; set; }
    }
}

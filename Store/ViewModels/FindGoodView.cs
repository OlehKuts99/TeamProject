using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.ViewModels
{
    public class FindGoodView
    {
        public string Name { get; set; }

        public string Specification { get; set; }

        public int? YearOfManufacture { get; set; }

        public decimal? Price { get; set; }

        public string Type { get; set; }
    }
}

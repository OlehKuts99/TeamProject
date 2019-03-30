using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Models;

namespace Store.ViewModels
{
    public class EditGoodView
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Specification { get; set; }

        public string PhotoUrl { get; set; }

        public int YearOfManufacture { get; set; }

        public int WarrantyTerm { get; set; }

        public int ProducerId { get; set; }

        public Producer Producer { get; set; }

        public decimal Price { get; set; }

        public string Type { get; set; }

        public int Count { get; set; }

        public ICollection<GoodStorage> Storages { get; set; }
    }
}

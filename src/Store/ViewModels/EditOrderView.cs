using DAL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.ViewModels
{
    public class EditOrderView
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerSurname { get; set; }

        public string CustomerEmail { get; set; }

        public DateTime? OrderDate { get; set; }

        public string EndPointCity { get; set; }

        public string EndPointStreet { get; set; }

        public bool SendEmail { get; set; }

        public OrderStatus Status { get; set; }
    }
}

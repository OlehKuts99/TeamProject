using System;
using System.Collections.Generic;
using Store.Classes;

namespace Store.ViewModels
{
    public class FindOrderView
    {
        public int? Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public List<OrderStatus> OrderStatuses { get; set; }
        public List<OrderStatus> SelectedStatuses { get; set; }
    }
}

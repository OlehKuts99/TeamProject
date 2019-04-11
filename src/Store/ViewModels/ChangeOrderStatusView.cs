using DAL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.ViewModels
{
    public class ChangeOrderStatusView
    {
        public int Id { get; set; }

        public OrderStatus CurrentStatus { get; set; }

        public List<OrderStatus> OrderStatuses { get; set; }

        public bool SendEmail { get; set; }
    }
}

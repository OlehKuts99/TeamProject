using System;
using System.Collections.Generic;

namespace Store.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public ICollection<Good> Products { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
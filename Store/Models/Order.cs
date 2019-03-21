using System;
using System.Collections.Generic;

namespace Store.Models
{
    /// <summary>
    /// Order model for create entity in database
    /// </summary>
    public class Order
    {
        public int Id { get; set; }

        public ICollection<Good> Products { get; set; }

        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }
    }
}
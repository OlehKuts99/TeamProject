﻿using Store.Classes;
using System;
using System.Collections.Generic;

namespace Store.Models
{
    /// <summary>
    /// Order model for create entity in database
    /// </summary>
    public class Order
    {
        public Order()
        {
            Products = new List<GoodOrder>();
        }

        public int Id { get; set; }

        public ICollection<GoodOrder> Products { get; set; }

        public DateTime OrderDate { get; set; }
        
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
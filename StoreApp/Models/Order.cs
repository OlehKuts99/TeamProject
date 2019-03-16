﻿using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public ICollection<Good> Products { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
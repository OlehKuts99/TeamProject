﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Models
{
    public class GoodCart
    {
        public int GoodId { get; set; }
        public Good Good { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}

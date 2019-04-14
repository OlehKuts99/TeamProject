using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class OrderPart
    {
        public int GoodId { get; set; }

        public Good Good { get; set; }

        public int Count { get; set; }
    }
}

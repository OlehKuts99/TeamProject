using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class GoodStorage
    {
        public int GoodId { get; set; }
        public Good Good { get; set; }

        public int StorageId { get; set; }
        public Storage Storage { get; set; }
    }
}

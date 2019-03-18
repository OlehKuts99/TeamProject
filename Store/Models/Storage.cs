using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Models
{
    /// <summary>
    /// Storage model for create entity in database
    /// </summary>
    public class Storage
    {
        public int Id { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public ICollection<Good> Products { get; set; }

    }
}

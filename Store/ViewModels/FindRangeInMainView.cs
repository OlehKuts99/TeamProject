using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.ViewModels
{
    public class FindRangeInMainView
    {
        public FindGoodView GoodView { get; set; }

        public IEnumerable<Good> list { get; set; }
    }
}

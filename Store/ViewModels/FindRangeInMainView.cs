using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.ViewModels
{
    public class FindRangeInMainView
    {
        public FindRangeInMainView()
        {
            this.Types = new List<string>
            {
                "All"
            };
        }

        public FindGoodView GoodView { get; set; }

        public IEnumerable<Good> List { get; set; }

        public List<string> Types { get; set; }
    }
}

using Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.ViewModels
{
    public class IndexViewModel
    {
        public FindRangeInMainView FilterModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}

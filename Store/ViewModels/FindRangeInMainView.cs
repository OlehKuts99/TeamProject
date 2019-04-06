using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotion.Linq.Clauses;
using Store.Classes.UnitOfWork;

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

        public FindRangeInMainView(UnitOfWork unitOfWork)
        {
            this.Types = new List<string>
            {
                "All"
            };

            var allTypes = unitOfWork.Goods.GetAll()
                .Select(p => p.Type)
                .Distinct();
            this.Types.AddRange(allTypes);
        }

        public FindGoodView GoodView { get; set; }

        public IEnumerable<Good> List { get; set; }

        public List<string> Types { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Classes;
using DAL.Classes.UnitOfWork;
using DAL.Models;
using Remotion.Linq.Clauses;

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

            AllSort = new Dictionary<string, string>
            {
                { SortBy.PriceFromLower.ToString(), "Lowest price" },
                { SortBy.PriceFromBigger.ToString(), "Highest price" },
                { SortBy.Popularity.ToString(), "Most popular" },
            };
        }

        public FindRangeInMainView(UnitOfWork unitOfWork) : this()
        {
            var allTypes = unitOfWork.Goods.GetAll()
                .Select(p => p.Type)
                .Distinct();
            this.Types.AddRange(allTypes);
        }

        public FindGoodView GoodView { get; set; }

        public IEnumerable<Good> Goods { get; set; }

        public List<string> Types { get; set; }

        public string ChoosenType { get; set; }

        public SortBy SortBy { get; set; }

        public Dictionary<string, string> AllSort { get; set; }
    }
}

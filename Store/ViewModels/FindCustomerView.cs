
using System.ComponentModel.DataAnnotations;

namespace Store.ViewModels
{
    public class FindCustomerView
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public int? Phone { get; set; }

        public string Email { get; set; }
    }
}

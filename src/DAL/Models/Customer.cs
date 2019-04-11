using System.Collections.Generic;

namespace DAL.Models
{
    /// <summary>
    /// User model for create entity in database.
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Cart Cart { get; set; }

        public ICollection<GoodReview> Reviews { get; set; }
    }
}

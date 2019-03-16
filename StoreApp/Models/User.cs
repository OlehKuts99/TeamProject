namespace StoreApp.Models
{
    /// <summary>
    /// User model for create entity in database.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public int Phone { get; set; }

        public string Email { get; set; }
    }
}

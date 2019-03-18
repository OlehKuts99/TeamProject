namespace Store.Models
{
    /// <summary>
    /// Producer model for create entity in database
    /// </summary>
    public class Producer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Phone { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

    }
}

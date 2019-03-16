namespace StoreApp.Models
{
    /// <summary>
    /// Producer model for create entity in database
    /// </summary>
    public class Producer
    {
        public int ProducerId { get; set; }

        public string Name { get; set; }

        public int Phone { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

    }
}

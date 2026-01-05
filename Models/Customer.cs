namespace TrendyProducts.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}

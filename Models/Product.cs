using System;
namespace TrendyProducts.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public decimal Price { get; set; }
        public string? Currency { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
        public int SalesCount { get; set; }
        public decimal Rating { get; set; }
    }
}


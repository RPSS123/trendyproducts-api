using System;
namespace TrendyProducts.DTOs
{
    public class ProductDTOs
    {
    public record ProductListDto(int Id, string Title, string Slug, decimal Price, string Currency, string ImageUrl, int SalesCount);

        public record ProductDetailDto(int Id, string Title, string Slug, string Description, decimal Price, string Currency, string ImageUrl, int SalesCount, decimal Rating, int Stock, int CategoryId);

        public record ProductCreateDto(string Title, string Slug, string Description, decimal Price, string Currency, string ImageUrl, int CategoryId, int Stock);
    }
}


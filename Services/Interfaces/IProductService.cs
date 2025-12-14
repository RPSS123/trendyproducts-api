using TrendyProducts.DTOs;
using System.Collections.Generic;

namespace TrendyProducts.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductDTOs.ProductListDto?> GetProducts(string? categorySlug, int page, int pageSize, string? sort);
        ProductDTOs.ProductDetailDto? GetProductBySlug(string slug);
        IEnumerable<ProductDTOs.ProductListDto> GetTopSellingByCategory(int categoryId, int limit);
        int CreateProduct(ProductDTOs.ProductCreateDto dto);
        IEnumerable<ProductDTOs.ProductListDto> SearchProducts(string term, int page, int pageSize);
    }
}
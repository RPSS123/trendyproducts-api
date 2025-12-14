using TrendyProducts.Repositories.Interfaces;
using TrendyProducts.Services.Interfaces;
using TrendyProducts.DTOs;
using System.Linq;
using TrendyProducts.Repositories.ADONET;

namespace TrendyProducts.Services
{
    public class ProductService : ICategoryService, IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ICategoryRepository _catRepo;
        public ProductService(IProductRepository repo, ICategoryRepository catRepo)
        {
            _repo = repo;
            _catRepo = catRepo;
        }

        public IEnumerable<ProductDTOs.ProductListDto> GetProducts(string? categorySlug, int page, int pageSize, string? sort)
        {
            var products = _repo.GetProducts(categorySlug, page, pageSize, sort);
            return products.Select(p => new ProductDTOs.ProductListDto(p.Id, p.Title, p.Slug, p.Price, p.Currency, p.ImageUrl, p.SalesCount));
        }

        public ProductDTOs.ProductDetailDto? GetProductBySlug(string slug)
        {
            var p = _repo.GetBySlug(slug);
            if (p == null) return null;
            return new ProductDTOs.ProductDetailDto(p.Id, p.Title, p.Slug, p.Description, p.Price, p.Currency, p.ImageUrl, p.SalesCount, p.Rating, p.Stock, p.CategoryId);
        }

        public IEnumerable<ProductDTOs.ProductListDto> GetTopSellingByCategory(int categoryId, int limit)
        {
            var list = _repo.GetTopSellingByCategory(categoryId, limit);
            return list.Select(p => new ProductDTOs.ProductListDto(p.Id, p.Title, p.Slug, p.Price, p.Currency, p.ImageUrl, p.SalesCount));
        }

        public int CreateProduct(ProductDTOs.ProductCreateDto dto)
        {
            // Basic create path for demo - convert DTO to Model
            var model = new TrendyProducts.Models.Product
            {
                Title = dto.Title,
                Slug = dto.Slug,
                Description = dto.Description,
                Price = dto.Price,
                Currency = dto.Currency,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                Stock = dto.Stock,
                Brand = "",
                SalesCount = 1000,
                Rating = 5m
            };
            return _repo.Create(model);
        }

        public IEnumerable<ProductDTOs.ProductListDto> SearchProducts(string term, int page, int pageSize)
        {
            var products = _repo.SearchProducts(term, page, pageSize);

            // same mapping style as GetProducts
            return products.Select(p => new ProductDTOs.ProductListDto(
                p.Id,
                p.Title,
                p.Slug,
                p.Price,
                p.Currency,
                p.ImageUrl,
                p.SalesCount
            ));
        }
    }
}
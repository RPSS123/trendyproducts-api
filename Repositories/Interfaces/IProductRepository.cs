
using System.Collections.Generic;
using TrendyProducts.Models;

namespace TrendyProducts.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts(string? category = null, int page = 1, int pageSize = 20, string? sort = null);
        Product GetById(int id);
        Product GetBySlug(string slug);
        IEnumerable<Product> GetTopSellingByCategory(int categoryId, int limit);
        int Create(Product product);
        void Update(Product product);
        void Delete(int id);
        IEnumerable<Product> SearchProducts(string term, int page, int pageSize);
    }
}
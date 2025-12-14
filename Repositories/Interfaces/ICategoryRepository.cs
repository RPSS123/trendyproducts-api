
using System.Collections.Generic;
using TrendyProducts.Models;

namespace TrendyProducts.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        Category GetBySlug(string slug);
        int Create(Category category);
        void Update(Category category);
        void Delete(int id);
    }
}
using System;
using TrendyProducts.Repositories.Interfaces;
using TrendyProducts.Services.Interfaces;

namespace TrendyProducts.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }
    }
}


using System.Collections.Generic;
using TrendyProducts.Models;
using TrendyProducts.Repositories.Interfaces;
using TrendyProducts.Services.Interfaces;

namespace TrendyProducts.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;
        public CartService(ICartRepository repo) => _repo = repo;

        public void AddToCart(int userId, int productId, int quantity)
        {
            _repo.AddOrUpdate(userId, productId, quantity);
        }

        public IEnumerable<CartItem> GetCart(int userId)
        {
            return _repo.GetCart(userId);
        }
    }
}

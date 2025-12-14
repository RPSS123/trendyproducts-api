using System.Collections.Generic;
using TrendyProducts.Models;

namespace TrendyProducts.Services.Interfaces
{
    public interface ICartService
    {
        void AddToCart(int userId, int productId, int quantity);
        IEnumerable<CartItem> GetCart(int userId);
    }
}

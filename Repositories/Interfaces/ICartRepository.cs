using System.Collections.Generic;
using TrendyProducts.Models;

namespace TrendyProducts.Repositories.Interfaces
{
    public interface ICartRepository
    {
        int AddOrUpdate(int userId, int productId, int quantity);
        IEnumerable<CartItem> GetCart(int userId);
    }
}
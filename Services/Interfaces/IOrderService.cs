using TrendyProducts.DTOs;

namespace TrendyProducts.Services.Interfaces
{
    public interface IOrderService
    {
        int CreateOrder(int customerId, int? userId, List<OrderItemDTO> items);
    }

}

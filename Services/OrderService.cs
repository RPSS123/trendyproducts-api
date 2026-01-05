using TrendyProducts.DTOs;
using TrendyProducts.Helpers;
using TrendyProducts.Services.Interfaces;

namespace TrendyProducts.Services
{
    public class OrderService : IOrderService
    {
        private readonly DbHelper _db;

        public OrderService(DbHelper db)
        {
            _db = db;
        }

        public int CreateOrder(int customerId, int? userId, List<OrderItemDTO> items)
        {
            decimal total = 0;

            foreach (var item in items)
            {
                var price = _db.ExecuteScalar<decimal>(
                    "SELECT Price FROM Products WHERE Id=@Id",
                    new { Id = item.ProductId });

                total += price * item.Quantity;
            }

            int orderId = _db.ExecuteScalar<int>(
                @"INSERT INTO Orders (CustomerId, UserId, TotalAmount)
              OUTPUT INSERTED.Id
              VALUES (@CustomerId, @UserId, @TotalAmount)",
                new { CustomerId = customerId, UserId = userId, TotalAmount = total });

            foreach (var item in items)
            {
                var price = _db.ExecuteScalar<decimal>(
                    "SELECT Price FROM Products WHERE Id=@Id",
                    new { Id = item.ProductId });

                _db.Execute(
                    @"INSERT INTO OrderItems
                  (OrderId, ProductId, PriceAtPurchase, Quantity)
                  VALUES (@OrderId, @ProductId, @Price, @Qty)",
                    new
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        Price = price,
                        Qty = item.Quantity
                    });
            }

            return orderId;
        }
    }

}

using System.Data;
using TrendyProducts.DTOs;
using TrendyProducts.Helpers;

namespace TrendyProducts.Repositories.ADONET
{
    public class AdminOrderRepository
    {
        private readonly DbHelper _db;

        public AdminOrderRepository(DbHelper db)
        {
            _db = db;
        }

        public IEnumerable<AdminOrderDTO> GetAllOrders()
        {
            return _db.Query<AdminOrderDTO>(
                "sp_GetAllOrders",
                commandType: CommandType.StoredProcedure
            );
        }

        public AdminOrderDetailDTO GetOrderDetails(int orderId)
        {
            return _db.QuerySingle<AdminOrderDetailDTO>(
                "sp_GetOrderDetails",
                new { OrderId = orderId },
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<AdminOrderItemDTO> GetOrderItems(int orderId)
        {
            return _db.Query<AdminOrderItemDTO>(
                "sp_GetOrderItems",
                new { OrderId = orderId },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            _db.Execute(
                "sp_UpdateOrderStatus",
                new { OrderId = orderId, Status = status },
                commandType: CommandType.StoredProcedure
            );
        }
    }

}

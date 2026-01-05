using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrendyProducts.Repositories.ADONET;

namespace TrendyProducts.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/orders")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly AdminOrderRepository _repo;

        public AdminOrdersController(AdminOrderRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAllOrders());
        }

        [HttpGet("{orderId}")]
        public IActionResult GetDetails(int orderId)
        {
            return Ok(new
            {
                order = _repo.GetOrderDetails(orderId),
                items = _repo.GetOrderItems(orderId)
            });
        }

        [HttpPut("{orderId}/status")]
        public IActionResult UpdateStatus(int orderId, [FromBody] string status)
        {
            _repo.UpdateOrderStatus(orderId, status);
            return Ok();
        }
    }

}

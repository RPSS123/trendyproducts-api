using Microsoft.AspNetCore.Mvc;
using TrendyProducts.DTOs;
using TrendyProducts.Services.Interfaces;

namespace TrendyProducts.Controllers
{
    [ApiController]
    [Route("api/checkout")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;

        public CheckoutController(
            ICustomerService customerService,
            IOrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
        }

        [HttpPost]
        public IActionResult Checkout(CreateOrderDTO dto)
        {
            int customerId = _customerService.SaveCustomer(dto.Customer);

            int orderId = _orderService.CreateOrder(
                customerId,
                dto.Customer.UserId,
                dto.Items);

            return Ok(new { orderId });
        }
    }

}

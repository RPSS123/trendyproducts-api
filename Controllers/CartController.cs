using Microsoft.AspNetCore.Mvc;
using TrendyProducts.Services.Interfaces;

namespace TrendyProducts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service) => _service = service;

        public record AddToCartDto(int UserId, int ProductId, int Quantity);

        [HttpPost]
        public IActionResult Add(AddToCartDto dto)
        {
            if (dto.Quantity <= 0)
                return BadRequest("Quantity must be > 0");

            _service.AddToCart(dto.UserId, dto.ProductId, dto.Quantity);
            return Ok(new { message = "Added to cart" });
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int userId)
        {
            var items = _service.GetCart(userId);
            return Ok(items);
        }
    }
}

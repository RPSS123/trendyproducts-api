using Microsoft.AspNetCore.Mvc;
using TrendyProducts.DTOs;
using TrendyProducts.Services.Interfaces;

namespace TrendyProducts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service) { _service = service; }

        [HttpGet]
        public IActionResult Get([FromQuery] string? category, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? sort = null)
        {
            var result = _service.GetProducts(category, page, pageSize, sort);
            return Ok(result);
        }

        [HttpGet("{slug}")]
        public IActionResult GetBySlug(string slug)
        {
            var p = _service.GetProductBySlug(slug);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpGet("category/{categoryId}/top")]
        public IActionResult TopByCategory(int categoryId, [FromQuery] int limit = 10)
        {
            var list = _service.GetTopSellingByCategory(categoryId, limit);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProductDTOs.ProductCreateDto dto)
        {
            if (dto == null) return BadRequest();

            try
            {
                var newId = _service.CreateProduct(dto);
                // If you have a GetById action, return CreatedAtAction:
                // return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
                return Ok(new { id = newId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // using TrendyProducts.Services.Interfaces;

        [HttpGet("search")]
        public IActionResult Search(
            [FromQuery] string q,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Search term (q) is required");

            var result = _service.SearchProducts(q, page, pageSize);
            return Ok(result);
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using TrendyProducts.Repositories.Interfaces;

namespace TrendyProducts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repo;
        public CategoriesController(ICategoryRepository repo) { _repo = repo; }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cats = _repo.GetAll();
            return Ok(cats);
        }

        [HttpGet("{slug}")]
        public IActionResult GetBySlug(string slug)
        {
            var c = _repo.GetBySlug(slug);
            if (c == null) return NotFound();
            return Ok(c);
        }
    }
}
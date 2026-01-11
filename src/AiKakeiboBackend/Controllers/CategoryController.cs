using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("GetCategoryData")]
        public async Task<IActionResult> GetCategoryDataAsync([FromQuery] int? UserId, [FromQuery] bool? DefaultFlg)
        {
            return await _service.GetCategoryDataAsync(UserId, DefaultFlg);
        }

        [HttpPost("RegistCategory")]
        public async Task<IActionResult> RegistCategoryAsync([FromBody] RegistCategoryRequest req)
        {
            return await _service.RegistCategoryAsync(req);
        }

        [HttpPost("UpdateCategory")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] UpdateCategoryRequest req)
        {
            return await _service.UpdateCategoryAsync(req);
        }
    }
}

using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IconController : ControllerBase
    {
        private readonly IIconService _service;

        public IconController(IIconService service)
        {
            _service = service;
        }

        [HttpGet("GetIconList")]
        public async Task<IActionResult> GetIconListAsync()
        {
            return await _service.GetIconListAsync();
        }
    }
}

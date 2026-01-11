using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KakeiboController : ControllerBase
    {
        private readonly IKakeiboService _service;

        public KakeiboController(IKakeiboService service)
        {
            _service = service;
        }

        [HttpPost("UpdateKakeibo")]
        public async Task<IActionResult> UpdateKakeiboAsync([FromBody] UpdateKakeiboRequest req)
        {
            return await _service.UpdateKakeiboAsync(req);
        }

        [HttpGet("GetMonthlyResult")]
        public async Task<IActionResult> GetMonthlyResultAsync([FromQuery] int UserId)
        {
            return await _service.GetMonthlyResultAsync(UserId);
        }

        [HttpGet("GetKakeiboItemList")]
        public async Task<IActionResult> GetKakeiboItemListAsync([FromQuery] int UserId, [FromQuery] string Range)
        {
            return await _service.GetKakeiboItemListAsync(UserId, Range);
        }

        [HttpGet("GetKakeiboItemDetail")]
        public async Task<IActionResult> GetKakeiboItemDetailAsync([FromQuery] int ItemId)
        {
            return await _service.GetKakeiboItemDetailAsync(ItemId);
        }

        [HttpPost("RegistKakeiboItem")]
        public async Task<IActionResult> RegistKakeiboItemAsync([FromBody] RegistKakeiboItemRequest req)
        {
            return await _service.RegistKakeiboItemAsync(req);
        }

        [HttpPost("UpdateKakeiboItem")]
        public async Task<IActionResult> UpdateKakeiboItemAsync([FromBody] UpdateKakeiboItemRequest req)
        {
            return await _service.UpdateKakeiboItemAsync(req);
        }

        [HttpPost("DeleteKakeiboItem")]
        public async Task<IActionResult> DeleteKakeiboItemAsync([FromBody] DeleteKakeiboItemRequest req)
        {
            return await _service.DeleteKakeiboItemAsync(req);
        }
    }
}

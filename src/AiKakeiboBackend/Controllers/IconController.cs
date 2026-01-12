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

        /// <summary>
        /// アイコン一覧取得APIです。カテゴリに設定可能なアイコンの一覧を取得します。
        /// </summary>
        /// <returns>アイコン情報のリスト（IconId, IconName等）</returns>
        [HttpGet("GetIconList")]
        public async Task<IActionResult> GetIconListAsync()
        {
            return await _service.GetIconListAsync();
        }
    }
}

using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IconController : ControllerBase
    {
        private readonly KakeiboDbContext _context;

        public IconController(KakeiboDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// アイコン一覧取得
        /// </summary>
        [HttpGet("GetIconList")]
        public async Task<ActionResult<ApiResponse<IconListResponse>>> GetIconList()
        {
            try
            {
                var icons = await _context.Icons
                    .Where(i => i.DeleteDate == null)
                    .Select(i => new IconDto
                    {
                        Id = i.Id,
                        IconName = i.DefaultIconName,
                        IconPath = i.OfficialIconName
                    })
                    .ToListAsync();

                var response = new IconListResponse
                {
                    Icons = icons
                };

                return Ok(HttpResponseService.Ok(response));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<IconListResponse>($"アイコン一覧取得中にエラーが発生しました: {ex.Message}"));
            }
        }
    }
}

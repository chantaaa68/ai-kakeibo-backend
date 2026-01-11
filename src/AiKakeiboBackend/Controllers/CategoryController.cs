using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly KakeiboDbContext _context;

        public CategoryController(KakeiboDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// カテゴリデータ取得
        /// </summary>
        [HttpGet("GetCategoryData")]
        public async Task<ActionResult<ApiResponse<CategoryListResponse>>> GetCategoryData(
            [FromQuery] int? UserId,
            [FromQuery] bool? DefaultFlg)
        {
            try
            {
                var categories = new List<CategoryDto>();

                // ユーザー固有のカテゴリを取得
                if (UserId.HasValue)
                {
                    // UserIdからKakeiboを取得
                    var kakeibo = await _context.Kakeibos
                        .FirstOrDefaultAsync(k => k.UserId == UserId.Value && k.DeleteDate == null);

                    if (kakeibo != null)
                    {
                        var userCategories = await _context.Categories
                            .Include(c => c.Icon)
                            .Where(c => c.KakeiboID == kakeibo.Id && c.DeleteDate == null)
                            .Select(c => new CategoryDto
                            {
                                Id = c.Id,
                                CategoryName = c.CategoryName,
                                InoutFlg = c.InoutFlg,
                                IconName = c.Icon.DefaultIconName,
                                IconId = c.IconId
                            })
                            .ToListAsync();

                        categories.AddRange(userCategories);
                    }
                }

                // デフォルトカテゴリを取得
                if (DefaultFlg == true)
                {
                    var defaultCategories = await _context.CategoryDefaults
                        .Include(c => c.Icon)
                        .Where(c => c.DeleteDate == null)
                        .Select(c => new CategoryDto
                        {
                            Id = c.Id,
                            CategoryName = c.KategoryName,
                            InoutFlg = c.InoutFlg,
                            IconName = c.Icon.DefaultIconName,
                            IconId = c.IconId
                        })
                        .ToListAsync();

                    categories.AddRange(defaultCategories);
                }

                var response = new CategoryListResponse
                {
                    Categories = categories
                };

                return Ok(HttpResponseService.Ok(response));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<CategoryListResponse>($"カテゴリデータ取得中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// カテゴリ登録
        /// </summary>
        [HttpPost("RegistCategory")]
        public async Task<ActionResult<ApiResponse<object>>> RegistCategory([FromBody] RegistCategoryRequest request)
        {
            try
            {
                // UserIdからKakeiboIdを取得
                var kakeibo = await _context.Kakeibos
                    .FirstOrDefaultAsync(k => k.UserId == request.UserId && k.DeleteDate == null);

                if (kakeibo == null)
                {
                    return Ok(HttpResponseService.Fail<object>("家計簿が見つかりません"));
                }

                // IconNameからIconを取得
                var icon = await _context.Icons
                    .FirstOrDefaultAsync(i => i.DefaultIconName == request.IconName && i.DeleteDate == null);

                if (icon == null)
                {
                    return Ok(HttpResponseService.Fail<object>("指定されたアイコンが見つかりません"));
                }

                // カテゴリ作成
                var category = new Category
                {
                    KakeiboID = kakeibo.Id,
                    CategoryName = request.CategoryName,
                    InoutFlg = request.InoutFlg,
                    IconId = icon.Id,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return Ok(HttpResponseService.Ok<object>(null, "カテゴリを登録しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"カテゴリ登録中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// カテゴリ更新
        /// </summary>
        [HttpPost("UpdateCategory")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            try
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.Id && c.DeleteDate == null);

                if (category == null)
                {
                    return Ok(HttpResponseService.Fail<object>("カテゴリが見つかりません"));
                }

                // カテゴリ名の更新
                if (!string.IsNullOrEmpty(request.CategoryName))
                {
                    category.CategoryName = request.CategoryName;
                }

                // 出入金フラグの更新
                if (request.InoutFlg.HasValue)
                {
                    category.InoutFlg = request.InoutFlg.Value;
                }

                // アイコンの更新
                if (!string.IsNullOrEmpty(request.IconName))
                {
                    var icon = await _context.Icons
                        .FirstOrDefaultAsync(i => i.DefaultIconName == request.IconName && i.DeleteDate == null);

                    if (icon == null)
                    {
                        return Ok(HttpResponseService.Fail<object>("指定されたアイコンが見つかりません"));
                    }

                    category.IconId = icon.Id;
                }

                category.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return Ok(HttpResponseService.Ok<object>(null, "カテゴリを更新しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"カテゴリ更新中にエラーが発生しました: {ex.Message}"));
            }
        }
    }
}

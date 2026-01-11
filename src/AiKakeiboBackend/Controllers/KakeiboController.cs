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
    public class KakeiboController : ControllerBase
    {
        private readonly KakeiboDbContext _context;

        public KakeiboController(KakeiboDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 家計簿情報更新
        /// </summary>
        [HttpPost("UpdateKakeibo")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateKakeibo([FromBody] UpdateKakeiboRequest request)
        {
            try
            {
                var kakeibo = await _context.Kakeibos
                    .FirstOrDefaultAsync(k => k.Id == request.Id && k.DeleteDate == null);

                if (kakeibo == null)
                {
                    return Ok(HttpResponseService.Fail<object>("家計簿が見つかりません"));
                }

                kakeibo.KakeiboName = request.KakeiboName;
                kakeibo.KakeiboExplanation = request.KakeiboExplanation;
                kakeibo.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(HttpResponseService.Ok<object>(null, "家計簿情報を更新しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"家計簿情報更新中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// 月次結果取得
        /// </summary>
        [HttpGet("GetMonthlyResult")]
        public async Task<ActionResult<ApiResponse<MonthlyResultDto>>> GetMonthlyResult([FromQuery] int UserId)
        {
            try
            {
                // UserIdからKakeiboIdを取得
                var kakeibo = await _context.Kakeibos
                    .FirstOrDefaultAsync(k => k.UserId == UserId && k.DeleteDate == null);

                if (kakeibo == null)
                {
                    return Ok(HttpResponseService.Fail<MonthlyResultDto>("家計簿が見つかりません"));
                }

                var now = DateTime.UtcNow;
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1);

                // 当月のアイテムを取得
                var items = await _context.KakeiboItems
                    .Include(i => i.Category)
                    .Where(i => i.KakeiboId == kakeibo.Id
                                && i.DeleteDate == null
                                && i.UsedDate >= startOfMonth
                                && i.UsedDate < endOfMonth)
                    .ToListAsync();

                // 収入・支出の合計計算
                var totalIncome = items.Where(i => i.InoutFlg == true).Sum(i => i.ItemAmount);
                var totalExpense = items.Where(i => i.InoutFlg == false).Sum(i => i.ItemAmount);

                // カテゴリ別集計
                var categorySummaries = items
                    .GroupBy(i => new { i.CategoryId, i.Category.CategoryName, i.InoutFlg })
                    .Select(g => new CategorySummaryDto
                    {
                        CategoryId = g.Key.CategoryId,
                        CategoryName = g.Key.CategoryName,
                        InoutFlg = g.Key.InoutFlg,
                        TotalAmount = g.Sum(i => i.ItemAmount)
                    })
                    .ToList();

                var response = new MonthlyResultDto
                {
                    Year = now.Year,
                    Month = now.Month,
                    TotalIncome = totalIncome,
                    TotalExpense = totalExpense,
                    Balance = totalIncome - totalExpense,
                    CategorySummaries = categorySummaries
                };

                return Ok(HttpResponseService.Ok(response));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<MonthlyResultDto>($"月次結果取得中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// 家計簿アイテムリスト取得
        /// </summary>
        [HttpGet("GetKakeiboItemList")]
        public async Task<ActionResult<ApiResponse<KakeiboItemListResponse>>> GetKakeiboItemList(
            [FromQuery] int UserId,
            [FromQuery] string Range)
        {
            try
            {
                // UserIdからKakeiboIdを取得
                var kakeibo = await _context.Kakeibos
                    .FirstOrDefaultAsync(k => k.UserId == UserId && k.DeleteDate == null);

                if (kakeibo == null)
                {
                    return Ok(HttpResponseService.Fail<KakeiboItemListResponse>("家計簿が見つかりません"));
                }

                // Range（期間）のパース
                DateTime? startDate = null;
                DateTime? endDate = null;

                if (!string.IsNullOrEmpty(Range))
                {
                    // Range形式: "YYYY-MM" または "YYYY-MM-DD"
                    var parts = Range.Split('-');
                    if (parts.Length >= 2)
                    {
                        int year = int.Parse(parts[0]);
                        int month = int.Parse(parts[1]);
                        startDate = new DateTime(year, month, 1);
                        endDate = startDate.Value.AddMonths(1);
                    }
                }

                var query = _context.KakeiboItems
                    .Include(i => i.Category)
                    .Include(i => i.KakeiboItemFrequency)
                    .Where(i => i.KakeiboId == kakeibo.Id && i.DeleteDate == null);

                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(i => i.UsedDate >= startDate.Value && i.UsedDate < endDate.Value);
                }

                var items = await query
                    .OrderByDescending(i => i.UsedDate)
                    .Select(i => new KakeiboItemDto
                    {
                        Id = i.Id,
                        ItemName = i.ItemName ?? string.Empty,
                        ItemAmount = i.ItemAmount,
                        InoutFlg = i.InoutFlg,
                        UsedDate = i.UsedDate,
                        CategoryId = i.CategoryId,
                        CategoryName = i.Category.CategoryName,
                        Frequency = i.KakeiboItemFrequency.Frequency
                    })
                    .ToListAsync();

                var response = new KakeiboItemListResponse
                {
                    Items = items
                };

                return Ok(HttpResponseService.Ok(response));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<KakeiboItemListResponse>($"アイテムリスト取得中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// 家計簿アイテム詳細取得
        /// </summary>
        [HttpGet("GetKakeiboItemDetail")]
        public async Task<ActionResult<ApiResponse<KakeiboItemDetailDto>>> GetKakeiboItemDetail([FromQuery] int ItemId)
        {
            try
            {
                var item = await _context.KakeiboItems
                    .Include(i => i.Category)
                    .Include(i => i.KakeiboItemFrequency)
                    .FirstOrDefaultAsync(i => i.Id == ItemId && i.DeleteDate == null);

                if (item == null)
                {
                    return Ok(HttpResponseService.Fail<KakeiboItemDetailDto>("アイテムが見つかりません"));
                }

                var response = new KakeiboItemDetailDto
                {
                    Id = item.Id,
                    ItemName = item.ItemName ?? string.Empty,
                    ItemAmount = item.ItemAmount,
                    InoutFlg = item.InoutFlg,
                    UsedDate = item.UsedDate,
                    CategoryId = item.CategoryId,
                    CategoryName = item.Category.CategoryName,
                    Frequency = item.KakeiboItemFrequency.Frequency,
                    FixedEndDate = item.KakeiboItemFrequency.FixedEndDate,
                    CreateDate = item.CreateDate,
                    UpdateDate = item.UpdateDate
                };

                return Ok(HttpResponseService.Ok(response));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<KakeiboItemDetailDto>($"アイテム詳細取得中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// 家計簿アイテム登録
        /// </summary>
        [HttpPost("RegistKakeiboItem")]
        public async Task<ActionResult<ApiResponse<object>>> RegistKakeiboItem([FromBody] RegistKakeiboItemRequest request)
        {
            try
            {
                // KakeiboIdの検証
                var kakeibo = await _context.Kakeibos
                    .FirstOrDefaultAsync(k => k.Id == request.KakeiboId && k.DeleteDate == null);

                if (kakeibo == null)
                {
                    return Ok(HttpResponseService.Fail<object>("家計簿が見つかりません"));
                }

                // カテゴリの検証
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.CategoryId && c.DeleteDate == null);

                if (category == null)
                {
                    return Ok(HttpResponseService.Fail<object>("カテゴリが見つかりません"));
                }

                // KakeiboItemFrequency作成
                var frequency = new KakeiboItemFrequency
                {
                    KakeiboId = request.KakeiboId,
                    CategoryId = request.CategoryId,
                    ItemName = request.ItemName,
                    ItemAmount = request.ItemAmount,
                    InoutFlg = request.InoutFlg,
                    Frequency = request.Frequency,
                    FixedStartDate = request.UsedDate,
                    FixedEndDate = request.FixedEndDate,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _context.KakeiboItemFrequencies.Add(frequency);
                await _context.SaveChangesAsync();

                // KakeiboItem作成
                var item = new KakeiboItem
                {
                    KakeiboId = request.KakeiboId,
                    CategoryId = request.CategoryId,
                    ItemName = request.ItemName,
                    ItemAmount = request.ItemAmount,
                    InoutFlg = request.InoutFlg,
                    UsedDate = request.UsedDate,
                    FrequencyId = frequency.Id,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _context.KakeiboItems.Add(item);
                await _context.SaveChangesAsync();

                return Ok(HttpResponseService.Ok<object>(null, "アイテムを登録しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"アイテム登録中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// 家計簿アイテム更新
        /// </summary>
        [HttpPost("UpdateKakeiboItem")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateKakeiboItem([FromBody] UpdateKakeiboItemRequest request)
        {
            try
            {
                var item = await _context.KakeiboItems
                    .Include(i => i.KakeiboItemFrequency)
                    .FirstOrDefaultAsync(i => i.Id == request.ItemId && i.DeleteDate == null);

                if (item == null)
                {
                    return Ok(HttpResponseService.Fail<object>("アイテムが見つかりません"));
                }

                // カテゴリの検証
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.CategoryId && c.DeleteDate == null);

                if (category == null)
                {
                    return Ok(HttpResponseService.Fail<object>("カテゴリが見つかりません"));
                }

                // アイテム情報更新
                item.CategoryId = request.CategoryId;
                item.ItemName = request.ItemName;
                item.ItemAmount = request.ItemAmount;
                item.InoutFlg = request.InoutFlg;
                item.UsedDate = request.UsedDate;
                item.UpdateDate = DateTime.UtcNow;

                // changeFlagがtrueの場合、Frequency情報も更新
                if (request.ChangeFlg && item.KakeiboItemFrequency != null)
                {
                    item.KakeiboItemFrequency.CategoryId = request.CategoryId;
                    item.KakeiboItemFrequency.ItemName = request.ItemName;
                    item.KakeiboItemFrequency.ItemAmount = request.ItemAmount;
                    item.KakeiboItemFrequency.InoutFlg = request.InoutFlg;
                    item.KakeiboItemFrequency.UpdateDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                return Ok(HttpResponseService.Ok<object>(null, "アイテムを更新しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"アイテム更新中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// 家計簿アイテム削除（論理削除）
        /// </summary>
        [HttpPost("DeleteKakeiboItem")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteKakeiboItem([FromBody] DeleteKakeiboItemRequest request)
        {
            try
            {
                var item = await _context.KakeiboItems
                    .FirstOrDefaultAsync(i => i.Id == request.Id && i.DeleteDate == null);

                if (item == null)
                {
                    return Ok(HttpResponseService.Fail<object>("アイテムが見つかりません"));
                }

                // 論理削除
                item.DeleteDate = DateTime.UtcNow;
                item.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(HttpResponseService.Ok<object>(null, "アイテムを削除しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"アイテム削除中にエラーが発生しました: {ex.Message}"));
            }
        }
    }
}

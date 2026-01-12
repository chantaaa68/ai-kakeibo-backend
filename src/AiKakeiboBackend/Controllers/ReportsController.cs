using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Controllers
{
    /// <summary>
    /// レポート関連のAPIエンドポイントを提供するコントローラー
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        /// <summary>
        /// カテゴリ別トレンドレポート取得APIです。
        /// 指定されたカテゴリの全期間の月別推移データと、指定月の取引明細を取得します。
        /// targetDateパラメータを省略した場合は、現在の月の取引明細を取得します。
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <param name="targetDate">対象日（yyyy-MM-dd形式、省略可能）</param>
        /// <returns>カテゴリ別トレンドレポート（カテゴリ名、今月の合計金額、全期間の月別推移データ、指定月の取引明細）</returns>
        [HttpGet("category-trend/{categoryId}")]
        public async Task<IActionResult> GetCategoryTrendReportAsync(int categoryId, [FromQuery] DateTime? targetDate)
        {
            return await _service.GetCategoryTrendReportAsync(categoryId, targetDate);
        }
    }
}

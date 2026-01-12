using AiKakeiboBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    /// <summary>
    /// レポート関連のビジネスロジックを提供するサービスインターフェース
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// カテゴリ別トレンドレポートを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <param name="targetDate">対象日（省略時は現在の月）</param>
        /// <returns>カテゴリ別トレンドレポート</returns>
        Task<IActionResult> GetCategoryTrendReportAsync(int categoryId, DateTime? targetDate);
    }
}

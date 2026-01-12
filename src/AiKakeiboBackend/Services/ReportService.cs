using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    /// <summary>
    /// レポート関連のビジネスロジックを提供するサービスクラス
    /// </summary>
    [Service]
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        /// <summary>
        /// カテゴリ別トレンドレポートを取得します。
        /// 指定されたカテゴリの全期間の月別推移データと、指定月の取引明細を返します。
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <param name="targetDate">対象日（省略時は現在の月）</param>
        /// <returns>カテゴリ別トレンドレポートを含むApiResponse。カテゴリが存在しない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> GetCategoryTrendReportAsync(int categoryId, DateTime? targetDate)
        {
            try
            {
                // カテゴリの存在確認
                Category? category = await _reportRepository.GetCategoryByIdAsync(categoryId);

                if (category == null)
                {
                    return ApiResponseHelper.Fail("カテゴリが見つかりません");
                }

                // targetDateがnullの場合は現在の月をデフォルトとする
                DateTime targetMonth = targetDate ?? DateTime.UtcNow;
                DateTime startOfTargetMonth = new DateTime(targetMonth.Year, targetMonth.Month, 1);
                DateTime endOfTargetMonth = startOfTargetMonth.AddMonths(1);

                // カテゴリIDに紐づく全期間の取引データを取得
                List<KakeiboItem> allItems = await _reportRepository.GetItemsByCategoryIdAsync(categoryId);

                // トレンドデータの生成
                List<TrendItem> trends = GenerateTrends(allItems);

                // 指定月の取引明細を取得
                List<KakeiboItem> targetMonthItems = await _reportRepository.GetItemsByCategoryIdAndRangeAsync(
                    categoryId,
                    startOfTargetMonth,
                    endOfTargetMonth
                );

                // 指定月の合計金額を算出
                decimal totalAmountThisMonth = targetMonthItems.Sum(i => i.ItemAmount);

                // 取引明細をDTOに変換
                List<TransactionItem> transactions = targetMonthItems
                    .Select(i => new TransactionItem
                    {
                        Id = i.Id,
                        ItemName = i.ItemName ?? string.Empty,
                        ItemAmount = i.ItemAmount,
                        UsedDate = i.UsedDate,
                        InoutFlg = i.InoutFlg
                    })
                    .ToList();

                GetCategoryTrendReportResponse response = new GetCategoryTrendReportResponse
                {
                    CategoryName = category.CategoryName,
                    TotalAmountThisMonth = totalAmountThisMonth,
                    Trends = trends,
                    Transactions = transactions
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"カテゴリ別トレンドレポート取得中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 全期間の月別推移データを生成します。
        /// 最初のデータから最新月まで、データがない月も0円として埋めた連続データを生成します。
        /// </summary>
        /// <param name="items">取引データのリスト</param>
        /// <returns>月別推移データのリスト</returns>
        private List<TrendItem> GenerateTrends(List<KakeiboItem> items)
        {
            List<TrendItem> trends = new List<TrendItem>();

            if (items.Count == 0)
            {
                return trends;
            }

            // 月別に集計
            Dictionary<DateTime, decimal> monthlyData = items
                .GroupBy(i => new DateTime(i.UsedDate.Year, i.UsedDate.Month, 1))
                .ToDictionary(
                    g => g.Key,
                    g => (decimal)g.Sum(i => i.ItemAmount)
                );

            // 最初のデータと最新月を取得
            DateTime firstMonth = monthlyData.Keys.Min();
            DateTime lastMonth = DateTime.UtcNow;
            DateTime currentMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);

            // 最初の月から最新月まで全ての月を生成（データがない月は0円）
            DateTime month = firstMonth;
            while (month <= currentMonth)
            {
                decimal amount = monthlyData.ContainsKey(month) ? monthlyData[month] : 0;

                trends.Add(new TrendItem
                {
                    Label = $"{month.Year}年{month.Month}月",
                    Amount = amount,
                    YearMonth = month.ToString("yyyy-MM-dd")
                });

                month = month.AddMonths(1);
            }

            return trends;
        }
    }
}

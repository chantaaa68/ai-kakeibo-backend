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
        /// 指定されたカテゴリの全期間の月別集計データと、各月の取引明細を返します。
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <param name="targetDate">対象日（未使用、将来の拡張用）</param>
        /// <returns>カテゴリ別トレンドレポートを含むApiResponse。カテゴリが存在しない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> GetCategoryTrendReportAsync(int categoryId, DateTime? targetDate)
        {
            try
            {
                // カテゴリの存在確認（IconをInclude）
                Category? category = await _reportRepository.GetCategoryByIdAsync(categoryId);

                if (category == null)
                {
                    return ApiResponseHelper.Fail("カテゴリが見つかりません");
                }

                // カテゴリIDに紐づく全期間の取引データを取得
                List<KakeiboItem> allItems = await _reportRepository.GetItemsByCategoryIdAsync(categoryId);

                // 月別集計データの生成
                List<MonthlyTrendData> monthlyData = GenerateMonthlyData(allItems, category.Icon?.OfficialIconName ?? string.Empty);

                GetCategoryTrendReportResponse response = new GetCategoryTrendReportResponse
                {
                    CategoryName = category.CategoryName,
                    MonthlyData = monthlyData
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"カテゴリ別トレンドレポート取得中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 全期間の月別集計データを生成します。
        /// データがある月のみを返却します。
        /// </summary>
        /// <param name="items">取引データのリスト</param>
        /// <param name="iconName">カテゴリのアイコン名</param>
        /// <returns>月別集計データのリスト</returns>
        private List<MonthlyTrendData> GenerateMonthlyData(List<KakeiboItem> items, string iconName)
        {
            if (items.Count == 0)
            {
                return new List<MonthlyTrendData>();
            }

            // 月別にグループ化
            var monthlyGroups = items
                .GroupBy(i => new { i.UsedDate.Year, i.UsedDate.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month);

            List<MonthlyTrendData> monthlyData = new List<MonthlyTrendData>();

            foreach (var monthGroup in monthlyGroups)
            {
                // 該当月のデータを日別にグループ化してKakeiboItemInfo形式に変換
                List<KakeiboItemInfo> dailyItems = monthGroup
                    .GroupBy(i => i.UsedDate.Day)
                    .OrderBy(g => g.Key)
                    .Select(dayGroup => new KakeiboItemInfo
                    {
                        DayNo = dayGroup.Key,
                        Items = dayGroup.Select(i => new Item
                        {
                            ItemId = i.Id,
                            ItemName = i.ItemName ?? string.Empty,
                            ItemAmount = i.ItemAmount,
                            InoutFlg = i.InoutFlg,
                            UsedDate = i.UsedDate,
                            IconName = iconName
                        }).ToList()
                    })
                    .ToList();

                monthlyData.Add(new MonthlyTrendData
                {
                    YearMonth = $"{monthGroup.Key.Year:D4}-{monthGroup.Key.Month:D2}",
                    TotalAmount = monthGroup.Sum(i => i.ItemAmount),
                    Items = dailyItems
                });
            }

            return monthlyData;
        }
    }
}

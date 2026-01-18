using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // Report リクエストDTO

    /// <summary>
    /// カテゴリ別トレンドレポート取得リクエスト
    /// </summary>
    public class GetCategoryTrendReportRequest
    {
        /// <summary>
        /// カテゴリID
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// 対象日（yyyy-MM-dd形式、省略時は現在の月）
        /// </summary>
        public DateTime? TargetDate { get; set; }
    }

    // Report レスポンスDTO

    /// <summary>
    /// カテゴリ別トレンドレポート取得レスポンス
    /// </summary>
    public class GetCategoryTrendReportResponse
    {
        /// <summary>
        /// カテゴリ名
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// 月ごとの集計データリスト
        /// </summary>
        public List<MonthlyTrendData> MonthlyData { get; set; } = new();
    }

    /// <summary>
    /// 月ごとの集計データ
    /// </summary>
    public class MonthlyTrendData
    {
        /// <summary>
        /// 年月 (yyyy-MM形式)
        /// </summary>
        public string YearMonth { get; set; } = string.Empty;

        /// <summary>
        /// 月間の合計金額
        /// </summary>
        public int TotalAmount { get; set; }

        /// <summary>
        /// 該当月のデータリスト（日別にグループ化）
        /// </summary>
        public List<KakeiboItemInfo> Items { get; set; } = new();
    }
}

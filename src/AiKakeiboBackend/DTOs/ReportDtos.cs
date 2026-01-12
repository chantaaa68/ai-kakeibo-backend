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
        /// 今月の合計金額
        /// </summary>
        public decimal TotalAmountThisMonth { get; set; }

        /// <summary>
        /// 全期間の月別推移データ
        /// </summary>
        public List<TrendItem> Trends { get; set; } = new();

        /// <summary>
        /// 指定月の取引明細
        /// </summary>
        public List<TransactionItem> Transactions { get; set; } = new();
    }

    /// <summary>
    /// トレンド項目
    /// </summary>
    public class TrendItem
    {
        /// <summary>
        /// グラフ表示用ラベル（例: "2025年10月", "10月"）
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// 合計金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 年月（グラフクリック時に月を特定するための値、例: "2025-10-01"）
        /// </summary>
        public string YearMonth { get; set; } = string.Empty;
    }

    /// <summary>
    /// 取引明細項目
    /// </summary>
    public class TransactionItem
    {
        /// <summary>
        /// 取引ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 取引名
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 金額
        /// </summary>
        public int ItemAmount { get; set; }

        /// <summary>
        /// 使用日
        /// </summary>
        public DateTime UsedDate { get; set; }

        /// <summary>
        /// 収支フラグ (true: 収入, false: 支出)
        /// </summary>
        public bool InoutFlg { get; set; }
    }
}

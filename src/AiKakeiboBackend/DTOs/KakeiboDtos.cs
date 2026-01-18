using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // Kakeibo リクエストDTO

    /// <summary>
    /// 月次集計結果取得リクエスト
    /// </summary>
    public class GetMonthlyResultRequest
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 全期間月次集計結果取得リクエスト
    /// </summary>
    public class GetMonthlyReportRequest
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 家計簿アイテムリスト取得リクエスト
    /// </summary>
    public class GetKakeiboItemListRequest
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 範囲
        /// </summary>
        [Required]
        [MinLength(1)]
        public required string Range { get; set; }
    }

    /// <summary>
    /// 家計簿アイテム詳細取得リクエスト
    /// </summary>
    public class GetKakeiboItemDetailRequest
    {
        /// <summary>
        /// アイテムID
        /// </summary>
        [Required]
        public int ItemId { get; set; }
    }

    /// <summary>
    /// 家計簿更新リクエスト
    /// </summary>
    public class UpdateKakeiboRequest
    {
        /// <summary>
        /// 家計簿ID
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 家計簿名
        /// </summary>
        [MinLength(1)]
        public string? KakeiboName { get; set; }

        /// <summary>
        /// 家計簿説明
        /// </summary>
        [MinLength(1)]
        public string? KakeiboExplanation { get; set; }
    }

    /// <summary>
    /// 家計簿アイテム登録リクエスト
    /// </summary>
    public class RegistKakeiboItemRequest
    {
        /// <summary>
        /// 家計簿ID
        /// </summary>
        [Required]
        public int KakeiboId { get; set; }

        /// <summary>
        /// アイテム名
        /// </summary>
        [Required]
        [MinLength(1)]
        public required string ItemName { get; set; }

        /// <summary>
        /// アイテム金額
        /// </summary>
        [Required]
        public int ItemAmount { get; set; }

        /// <summary>
        /// 収支フラグ (true: 収入, false: 支出)
        /// </summary>
        [Required]
        public bool InoutFlg { get; set; }

        /// <summary>
        /// 使用日
        /// </summary>
        [Required]
        public DateTime UsedDate { get; set; }

        /// <summary>
        /// カテゴリーID
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// 頻度 (0: 都度, 1-11: 毎月の繰り返し)
        /// </summary>
        [Required]
        [Range(0, 11)]
        public int Frequency { get; set; }

        /// <summary>
        /// 固定終了日
        /// </summary>
        public DateTime? FixedEndDate { get; set; }
    }

    /// <summary>
    /// 家計簿アイテム更新リクエスト
    /// </summary>
    public class UpdateKakeiboItemRequest
    {
        /// <summary>
        /// アイテムID
        /// </summary>
        [Required]
        public int ItemId { get; set; }

        /// <summary>
        /// カテゴリーID
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// アイテム名
        /// </summary>
        [Required]
        [MinLength(1)]
        public required string ItemName { get; set; }

        /// <summary>
        /// アイテム金額
        /// </summary>
        [Required]
        public int ItemAmount { get; set; }

        /// <summary>
        /// 収支フラグ (true: 収入, false: 支出)
        /// </summary>
        [Required]
        public bool InoutFlg { get; set; }

        /// <summary>
        /// 使用日
        /// </summary>
        [Required]
        public DateTime UsedDate { get; set; }

        /// <summary>
        /// 変更フラグ (true: 全件変更, false: 単件変更)
        /// </summary>
        [Required]
        public bool ChangeFlg { get; set; }
    }

    /// <summary>
    /// 家計簿アイテム削除リクエスト
    /// </summary>
    public class DeleteKakeiboItemRequest
    {
        /// <summary>
        /// アイテムID
        /// </summary>
        [Required]
        public int Id { get; set; }
    }

    // Kakeibo レスポンスDTO

    /// <summary>
    /// 月次集計結果取得レスポンス
    /// </summary>
    public class GetMonthlyResultResponse
    {
        /// <summary>
        /// 月次支出リスト
        /// </summary>
        public List<MonthlyReportItem> MonthlyExpenses { get; set; } = new();
        /// <summary>
        /// 月次収入リスト
        /// </summary>
        public List<MonthlyReportItem> MonthlyIncomes { get; set; } = new();
    }

    /// <summary>
    /// 全期間月次集計結果レスポンス
    /// </summary>
    public class GetMonthlyReportResult
    {
        /// <summary>
        /// 月次支出リスト
        /// </summary>
        public List<MonthlyReportItem> MonthlyExpenses { get; set; } = new();
        /// <summary>
        /// 月次収入リスト
        /// </summary>
        public List<MonthlyReportItem> MonthlyIncomes { get; set; } = new();
    }

    /// <summary>
    /// 月次レポート（収入・支出別）
    /// </summary>
    public class MonthlyReport
    {
        /// <summary>
        /// 収支フラグ (true: 収入, false: 支出)
        /// </summary>
        public bool InoutFlg { get; set; }
        /// <summary>
        /// 月次レポート項目リスト
        /// </summary>
        public List<MonthlyReportItem> MonthlyReportItems { get; set; } = new();
    }

    /// <summary>
    /// 月次レポート項目
    /// </summary>
    public class MonthlyReportItem
    {
        /// <summary>
        /// 使用月 (YYYY-MM形式)
        /// </summary>
        public string UsedMonth { get; set; } = string.Empty;
        /// <summary>
        /// カテゴリー別レポート項目リスト
        /// </summary>
        public List<CategoryReportItem> CategoryReportItems { get; set; } = new();
    }

    /// <summary>
    /// カテゴリー別レポート項目
    /// </summary>
    public class CategoryReportItem
    {
        /// <summary>
        /// カテゴリID
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// カテゴリー名
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// アイコン名
        /// </summary>
        public string IconName { get; set; } = string.Empty;
        /// <summary>
        /// 合計金額
        /// </summary>
        public int TotalAmount { get; set; }
    }

    /// <summary>
    /// 家計簿アイテムリスト取得レスポンス
    /// </summary>
    public class GetKakeiboItemListResponse
    {
        /// <summary>
        /// 家計簿アイテム情報リスト
        /// </summary>
        public List<KakeiboItemInfo> KakeiboItemInfos { get; set; } = new();
    }

    /// <summary>
    /// 家計簿アイテム情報
    /// </summary>
    public class KakeiboItemInfo
    {
        /// <summary>
        /// 日付番号
        /// </summary>
        public int DayNo { get; set; }
        /// <summary>
        /// アイテムリスト
        /// </summary>
        public List<Item> Items { get; set; } = new();
    }

    /// <summary>
    /// アイテム
    /// </summary>
    public class Item
    {
        /// <summary>
        /// アイテムID
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// アイテム名
        /// </summary>
        public string ItemName { get; set; } = string.Empty;
        /// <summary>
        /// アイテム金額
        /// </summary>
        public int ItemAmount { get; set; }
        /// <summary>
        /// 収支フラグ (true: 収入, false: 支出)
        /// </summary>
        public bool InoutFlg { get; set; }
        /// <summary>
        /// 使用日
        /// </summary>
        public DateTime UsedDate { get; set; }
        /// <summary>
        /// アイコン名
        /// </summary>
        public string IconName { get; set; } = string.Empty;
    }

    /// <summary>
    /// 家計簿アイテム詳細取得レスポンス
    /// </summary>
    public class GetKakeiboItemDetailResponse
    {
        /// <summary>
        /// アイテム名
        /// </summary>
        public string ItemName { get; set; } = string.Empty;
        /// <summary>
        /// アイテム金額
        /// </summary>
        public int ItemAmount { get; set; }
        /// <summary>
        /// 収支フラグ (true: 収入, false: 支出)
        /// </summary>
        public bool InoutFlg { get; set; }
        /// <summary>
        /// 使用日
        /// </summary>
        public DateTime UsedDate { get; set; }
        /// <summary>
        /// カテゴリーID
        /// </summary>
        public int CategoryId { get; set; }
    }

    /// <summary>
    /// 家計簿更新レスポンス
    /// </summary>
    public class UpdateKakeiboResponse
    {
        /// <summary>
        /// 更新件数
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 家計簿アイテム登録レスポンス
    /// </summary>
    public class RegistKakeiboItemResponse
    {
        /// <summary>
        /// 登録件数
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 家計簿アイテム更新レスポンス
    /// </summary>
    public class UpdateKakeiboItemResponse
    {
        /// <summary>
        /// 更新件数
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 家計簿アイテム削除レスポンス
    /// </summary>
    public class DeleteKakeiboItemResponse
    {
        /// <summary>
        /// 削除件数
        /// </summary>
        public int Count { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // Kakeibo リクエストDTO

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
        [Required]
        [MinLength(1)]
        public required string KakeiboName { get; set; }

        /// <summary>
        /// 家計簿説明
        /// </summary>
        [Required]
        [MinLength(1)]
        public required string KakeiboExplanation { get; set; }
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
    /// 月次集計結果DTO
    /// </summary>
    public class MonthlyResultDto
    {
        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 収入合計
        /// </summary>
        public int TotalIncome { get; set; }
        /// <summary>
        /// 支出合計
        /// </summary>
        public int TotalExpense { get; set; }
        /// <summary>
        /// 収支差額
        /// </summary>
        public int Balance { get; set; }
        /// <summary>
        /// カテゴリー別集計リスト
        /// </summary>
        public List<CategorySummaryDto> CategorySummaries { get; set; } = new();
    }

    /// <summary>
    /// カテゴリー別集計DTO
    /// </summary>
    public class CategorySummaryDto
    {
        /// <summary>
        /// カテゴリーID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// カテゴリー名
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// 収支フラグ (true: 収入, false: 支出)
        /// </summary>
        public bool InoutFlg { get; set; }
        /// <summary>
        /// 合計金額
        /// </summary>
        public int TotalAmount { get; set; }
    }

    /// <summary>
    /// 家計簿アイテムリストレスポンス
    /// </summary>
    public class KakeiboItemListResponse
    {
        /// <summary>
        /// アイテムリスト
        /// </summary>
        public List<KakeiboItemDto> Items { get; set; } = new();
    }

    /// <summary>
    /// 家計簿アイテムDTO
    /// </summary>
    public class KakeiboItemDto
    {
        /// <summary>
        /// アイテムID
        /// </summary>
        public int Id { get; set; }
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
        /// <summary>
        /// カテゴリー名
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// 頻度 (0: 都度, 1-11: 毎月の繰り返し)
        /// </summary>
        public int Frequency { get; set; }
    }

    /// <summary>
    /// 家計簿アイテム詳細DTO
    /// </summary>
    public class KakeiboItemDetailDto
    {
        /// <summary>
        /// アイテムID
        /// </summary>
        public int Id { get; set; }
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
        /// <summary>
        /// カテゴリー名
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// 頻度 (0: 都度, 1-11: 毎月の繰り返し)
        /// </summary>
        public int Frequency { get; set; }
        /// <summary>
        /// 固定終了日
        /// </summary>
        public DateTime? FixedEndDate { get; set; }
        /// <summary>
        /// 作成日
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}

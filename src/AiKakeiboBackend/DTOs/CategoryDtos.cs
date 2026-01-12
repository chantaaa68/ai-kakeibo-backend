using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // Category リクエストDTO

    /// <summary>
    /// カテゴリーデータ取得リクエスト
    /// </summary>
    public class GetCategoryDataRequest
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// デフォルトフラグ (true: デフォルトカテゴリー, false: ユーザーカテゴリー)
        /// </summary>
        [Required]
        public bool DefaultFlg { get; set; }
    }

    /// <summary>
    /// カテゴリー登録リクエスト
    /// </summary>
    public class RegistCategoryRequest
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// カテゴリー名
        /// </summary>
        [Required]
        [MinLength(1)]
        public required string CategoryName { get; set; }

        /// <summary>
        /// 収支フラグ (true: 収入, false: 支出)
        /// </summary>
        [Required]
        public bool InoutFlg { get; set; }

        /// <summary>
        /// アイコン名
        /// </summary>
        [Required]
        [MinLength(1)]
        public required string IconName { get; set; }
    }

    /// <summary>
    /// カテゴリー更新リクエスト
    /// </summary>
    public class UpdateCategoryRequest
    {
        /// <summary>
        /// カテゴリーID
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// カテゴリー名
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// 収支フラグ (true: 収入, false: 支出)
        /// </summary>
        public bool? InoutFlg { get; set; }

        /// <summary>
        /// アイコン名
        /// </summary>
        public string? IconName { get; set; }
    }

    // Category レスポンスDTO

    /// <summary>
    /// カテゴリーデータ取得レスポンス
    /// </summary>
    public class GetCategoryDataResponse
    {
        /// <summary>
        /// カテゴリーリスト
        /// </summary>
        public List<CategoryItem>? Categories { get; set; }
    }

    /// <summary>
    /// カテゴリー項目
    /// </summary>
    public class CategoryItem
    {
        /// <summary>
        /// カテゴリーID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// カテゴリー名
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// 収支フラグ (true: 収入, false: 支出)
        /// </summary>
        public bool InoutFlg { get; set; }
        /// <summary>
        /// アイコン名
        /// </summary>
        public string IconName { get; set; } = string.Empty;
    }

    /// <summary>
    /// カテゴリー登録レスポンス
    /// </summary>
    public class RegistCategoryResponse
    {
        /// <summary>
        /// カテゴリーID
        /// </summary>
        public int CategoryId { get; set; }
    }

    /// <summary>
    /// カテゴリー更新レスポンス
    /// </summary>
    public class UpdateCategoryResponse
    {
        /// <summary>
        /// カテゴリーID
        /// </summary>
        public int CategoryId { get; set; }
    }
}

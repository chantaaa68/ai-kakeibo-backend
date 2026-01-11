using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // Category リクエストDTO

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
    /// カテゴリー情報DTO
    /// </summary>
    public class CategoryDto
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
        /// <summary>
        /// アイコンID
        /// </summary>
        public int IconId { get; set; }
    }

    /// <summary>
    /// カテゴリーリストレスポンス
    /// </summary>
    public class CategoryListResponse
    {
        /// <summary>
        /// カテゴリーリスト
        /// </summary>
        public List<CategoryDto> Categories { get; set; } = new();
    }
}

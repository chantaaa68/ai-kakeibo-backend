using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // Kakeibo リクエストDTO

    public class UpdateKakeiboRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public required string KakeiboName { get; set; }

        [Required]
        [MinLength(1)]
        public required string KakeiboExplanation { get; set; }
    }

    public class RegistKakeiboItemRequest
    {
        [Required]
        public int KakeiboId { get; set; }

        [Required]
        [MinLength(1)]
        public required string ItemName { get; set; }

        [Required]
        public int ItemAmount { get; set; }

        [Required]
        public bool InoutFlg { get; set; }

        [Required]
        public DateTime UsedDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Range(0, 11)]
        public int Frequency { get; set; }

        public DateTime? FixedEndDate { get; set; }
    }

    public class UpdateKakeiboItemRequest
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MinLength(1)]
        public required string ItemName { get; set; }

        [Required]
        public int ItemAmount { get; set; }

        [Required]
        public bool InoutFlg { get; set; }

        [Required]
        public DateTime UsedDate { get; set; }

        [Required]
        public bool ChangeFlg { get; set; }
    }

    public class DeleteKakeiboItemRequest
    {
        [Required]
        public int Id { get; set; }
    }

    // Kakeibo レスポンスDTO

    public class MonthlyResultDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalIncome { get; set; }
        public int TotalExpense { get; set; }
        public int Balance { get; set; }
        public List<CategorySummaryDto> CategorySummaries { get; set; } = new();
    }

    public class CategorySummaryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool InoutFlg { get; set; }
        public int TotalAmount { get; set; }
    }

    public class KakeiboItemListResponse
    {
        public List<KakeiboItemDto> Items { get; set; } = new();
    }

    public class KakeiboItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int ItemAmount { get; set; }
        public bool InoutFlg { get; set; }
        public DateTime UsedDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int Frequency { get; set; }
    }

    public class KakeiboItemDetailDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int ItemAmount { get; set; }
        public bool InoutFlg { get; set; }
        public DateTime UsedDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int Frequency { get; set; }
        public DateTime? FixedEndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

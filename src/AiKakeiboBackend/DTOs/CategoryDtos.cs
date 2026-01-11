using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // Category リクエストDTO

    public class RegistCategoryRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(1)]
        public required string CategoryName { get; set; }

        [Required]
        public bool InoutFlg { get; set; }

        [Required]
        [MinLength(1)]
        public required string IconName { get; set; }
    }

    public class UpdateCategoryRequest
    {
        [Required]
        public int Id { get; set; }

        public string? CategoryName { get; set; }

        public bool? InoutFlg { get; set; }

        public string? IconName { get; set; }
    }

    // Category レスポンスDTO

    public class CategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool InoutFlg { get; set; }
        public string IconName { get; set; } = string.Empty;
        public int IconId { get; set; }
    }

    public class CategoryListResponse
    {
        public List<CategoryDto> Categories { get; set; } = new();
    }
}

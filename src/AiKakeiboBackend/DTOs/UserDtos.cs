using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // User リクエストDTO

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [MinLength(1)]
        public required string Email { get; set; }

        [Required]
        [MinLength(1)]
        public required string UserHash { get; set; }
    }

    public class RegistUserRequest
    {
        [Required]
        [MinLength(1)]
        public required string UserName { get; set; }

        [Required]
        [MinLength(1)]
        public required string UserHash { get; set; }

        [Required]
        [MinLength(1)]
        public required string Email { get; set; }

        [Required]
        [MinLength(1)]
        public required string KakeiboName { get; set; }

        [Required]
        [MinLength(1)]
        public required string KakeiboExplanation { get; set; }
    }

    public class UpdateUserRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(1)]
        public required string UserName { get; set; }

        [Required]
        [MinLength(1)]
        public required string Email { get; set; }

        [Required]
        [MinLength(1)]
        public required string KakeiboName { get; set; }

        [Required]
        [MinLength(1)]
        public required string KakeiboExplanation { get; set; }
    }

    public class DeleteUserRequest
    {
        [Required]
        public int UserId { get; set; }
    }

    // User レスポンスDTO

    public class LoginResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public int KakeiboId { get; set; }
        public string KakeiboName { get; set; } = string.Empty;
    }

    public class UserDataResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int KakeiboId { get; set; }
        public string KakeiboName { get; set; } = string.Empty;
        public string KakeiboExplanation { get; set; } = string.Empty;
    }
}

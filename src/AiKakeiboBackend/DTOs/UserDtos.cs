using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // User リクエストDTO

    /// <summary>
    /// ログインリクエスト
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// メールアドレス
        /// </summary>
        [Required]
        [EmailAddress]
        [MinLength(1)]
        public required string Email { get; set; }

        /// <summary>
        /// ユーザーパスワードのハッシュ値
        /// </summary>
        [Required]
        [MinLength(1)]
        public required string UserHash { get; set; }
    }

    /// <summary>
    /// ユーザー登録リクエスト
    /// </summary>
    public class RegistUserRequest
    {
        /// <summary>
        /// ユーザー名
        /// </summary>
        [Required]
        [MinLength(1)]
        public required string UserName { get; set; }

        /// <summary>
        /// ユーザーパスワードのハッシュ値
        /// </summary>
        [Required]
        [MinLength(1)]
        public required string UserHash { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Required]
        [MinLength(1)]
        public required string Email { get; set; }

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
    /// ユーザー更新リクエスト
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        [MinLength(1)]
        public string? UserName { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [EmailAddress]
        [MinLength(1)]
        public string? Email { get; set; }

        /// <summary>
        /// ユーザーパスワードのハッシュ値
        /// </summary>
        [MinLength(1)]
        public string? UserHash { get; set; }

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
    /// ユーザー削除リクエスト
    /// </summary>
    public class DeleteUserRequest
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public int UserId { get; set; }
    }

    /// <summary>
    /// ユーザーデータ取得リクエスト
    /// </summary>
    public class GetUserDataRequest
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public int UserId { get; set; }
    }

    // User レスポンスDTO

    /// <summary>
    /// ログインレスポンス
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 認証トークン
        /// </summary>
        public string Token { get; set; } = string.Empty;
        /// <summary>
        /// 家計簿ID
        /// </summary>
        public int KakeiboId { get; set; }
    }

    /// <summary>
    /// ユーザーデータ取得レスポンス
    /// </summary>
    public class GetUserDataResponse
    {
        /// <summary>
        /// ユーザー名
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// メールアドレス
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// 家計簿名
        /// </summary>
        public string KakeiboName { get; set; } = string.Empty;
        /// <summary>
        /// 家計簿説明
        /// </summary>
        public string KakeiboExplanation { get; set; } = string.Empty;
    }

    /// <summary>
    /// ユーザー登録レスポンス
    /// </summary>
    public class RegistUserResponse
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        public int UserId { get; set; }
    }

    /// <summary>
    /// ユーザー削除レスポンス
    /// </summary>
    public class DeleteUserResponse
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        public int UserId { get; set; }
    }
}

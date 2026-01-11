using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // Newsletter リクエストDTO

    /// <summary>
    /// ニュースレター登録リクエスト
    /// </summary>
    public class RegistNewsletterRequest
    {
        /// <summary>
        /// タイトル
        /// </summary>
        [Required]
        public string? Title { get; set; }

        /// <summary>
        /// メール本文
        /// </summary>
        [Required]
        public string? MailBody { get; set; }
    }

    /// <summary>
    /// ニュースレター更新リクエスト
    /// </summary>
    public class UpdateNewsletterRequest
    {
        /// <summary>
        /// ニュースレターID
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        [Required]
        public string? Title { get; set; }

        /// <summary>
        /// メール本文
        /// </summary>
        [Required]
        public string? MailBody { get; set; }
    }

    /// <summary>
    /// ニュースレター送信リクエスト
    /// </summary>
    public class SendNewsletterRequest
    {
        /// <summary>
        /// ニュースレターID
        /// </summary>
        [Required]
        public int NewsletterId { get; set; }

        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// アイテムID
        /// </summary>
        [Required]
        public int ItemId { get; set; }
    }
}

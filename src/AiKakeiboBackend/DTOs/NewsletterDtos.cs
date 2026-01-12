using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // Newsletter リクエストDTO

    /// <summary>
    /// ニュースレター登録リクエスト
    /// </summary>
    public class RigistNewsletterRequest
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

    // Newsletter レスポンスDTO

    /// <summary>
    /// ニュースレター登録レスポンス
    /// </summary>
    public class RigistNewsletterResponse
    {
        /// <summary>
        /// 登録件数
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// ニュースレター更新レスポンス
    /// </summary>
    public class UpdateNewsletterResponse
    {
        /// <summary>
        /// 更新件数
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// ニュースレター送信レスポンス
    /// </summary>
    public class SendNewsletterResponse
    {
        /// <summary>
        /// 送信件数
        /// </summary>
        public int SendCount { get; set; }
    }
}

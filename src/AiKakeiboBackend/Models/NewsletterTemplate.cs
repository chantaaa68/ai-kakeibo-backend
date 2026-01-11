using AiKakeiboBackend.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.Models
{
    public class NewsletterTemplate : KakeiboInterface
    {
        [Key]
        [Required]
        [Comment("ID")]
        public int Id { get; set; }

        [Required]
        [Comment("メールタイトル")]
        [MaxLength(100)]
        public required string MailTitle { get; set; }

        [Required]
        [Comment("メール本文")]
        [MaxLength(5120)]
        public required string MailBody { get; set; }

        [Required]
        [Comment("登録日時")]
        public DateTime CreateDate { get; set; }

        [Required]
        [Comment("更新日時")]
        public DateTime UpdateDate { get; set; }

        [Comment("削除日時")]
        public DateTime? DeleteDate { get; set; }
    }
}

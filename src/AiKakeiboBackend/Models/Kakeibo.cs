using AiKakeiboBackend.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiKakeiboBackend.Models
{
    public class Kakeibo : KakeiboInterface
    {
        public Kakeibo()
        {
            KakeiboItem = new HashSet<KakeiboItem>();
            Category = new HashSet<Category>();
            KakeiboItemFrequency = new HashSet<KakeiboItemFrequency>();
        }

        [Key]
        [Required]
        [Comment("ID")]
        public int Id { get; set; }

        [Required]
        [Comment("ユーザーID")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [Comment("家計簿名")]
        [MaxLength(20)]
        public string KakeiboName { get; set; } = string.Empty!;

        [Comment("説明文")]
        [MaxLength(1024)]
        public string? KakeiboExplanation { get; set; }

        [Required]
        [Comment("登録日時")]
        public DateTime CreateDate { get; set; }

        [Required]
        [Comment("更新日時")]
        public DateTime UpdateDate { get; set; }

        [Comment("削除日時")]
        public DateTime? DeleteDate { get; set; }

        public virtual Users User { get; set; } = null!;

        public virtual ICollection<KakeiboItem> KakeiboItem { get; set; }

        public virtual ICollection<Category> Category { get; set; }

        public virtual ICollection<KakeiboItemFrequency> KakeiboItemFrequency { get; set; }
    }
}

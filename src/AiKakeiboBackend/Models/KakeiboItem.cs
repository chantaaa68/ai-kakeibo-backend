using AiKakeiboBackend.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiKakeiboBackend.Models
{
    public class KakeiboItem : KakeiboInterface
    {
        [Key]
        [Required]
        [Comment("ID")]
        public int Id { get; set; }

        [Required]
        [Comment("家計簿テーブルID")]
        [ForeignKey(nameof(Kakeibo))]
        public int KakeiboId { get; set; }

        [Required]
        [Comment("カテゴリID")]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        [Comment("名前")]
        [MaxLength(20)]
        public string? ItemName { get; set; }

        [Required]
        [Comment("金額")]
        public int ItemAmount { get; set; }

        [Required]
        [Comment("出入金フラグ")]
        public bool InoutFlg { get; set; }

        [Required]
        [Comment("出入金日付")]
        public DateTime UsedDate { get; set; }

        [Required]
        [Comment("固定費管理ID")]
        [ForeignKey(nameof(KakeiboItemFrequency))]
        public int FrequencyId { get; set; }

        [Required]
        [Comment("登録日時")]
        public DateTime CreateDate { get; set; }

        [Required]
        [Comment("更新日時")]
        public DateTime UpdateDate { get; set; }

        [Comment("削除日時")]
        public DateTime? DeleteDate { get; set; }

        public virtual Kakeibo Kakeibo { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;

        public virtual KakeiboItemFrequency KakeiboItemFrequency { get; set; } = null!;
    }
}

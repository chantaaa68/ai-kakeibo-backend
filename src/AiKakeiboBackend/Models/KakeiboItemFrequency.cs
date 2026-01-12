using AiKakeiboBackend.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiKakeiboBackend.Models
{
    public class KakeiboItemFrequency : KakeiboInterface
    {
        public KakeiboItemFrequency()
        {
            KakeiboItems = new HashSet<KakeiboItem>();
        }

        [Key]
        [Required]
        [Comment("ID")]
        public int Id { get; set; }

        [Required]
        [Comment("家計簿テーブルID")]
        [ForeignKey(nameof(Kakeibo))]
        public int KakeiboId { get; set; }

        [Comment("カテゴリID")]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Comment("名前")]
        [MaxLength(20)]
        public string? ItemName { get; set; }

        [Comment("金額")]
        public int ItemAmount { get; set; }

        [Comment("出入金フラグ")]
        public bool InoutFlg { get; set; }

        [Required]
        [Comment("固定費頻度")]
        [Range(0, 11)]
        public int Frequency { get; set; }

        [Comment("固定費開始日時")]
        [MaxLength(20)]
        public DateTime? FixedStartDate { get; set; }

        [Comment("固定費終了日時")]
        [MaxLength(20)]
        public DateTime? FixedEndDate { get; set; }

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

        public virtual ICollection<KakeiboItem> KakeiboItems { get; set; }
    }
}

using AiKakeiboBackend.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiKakeiboBackend.Models
{
    public class CategoryDefault : KakeiboInterface
    {
        [Key]
        [Required]
        [Comment("ID")]
        public int Id { get; set; }

        [Required]
        [Comment("カテゴリ名")]
        [MaxLength(20)]
        public string KategoryName { get; set; } = null!;

        [Required]
        [Comment("出入金フラグ")]
        public Boolean InoutFlg { get; set; }

        [Required]
        [Comment("アイコンID")]
        [ForeignKey(nameof(Icon))]
        public int IconId { get; set; }

        [Required]
        [Comment("登録日時")]
        public DateTime CreateDate { get; set; }

        [Required]
        [Comment("更新日時")]
        public DateTime UpdateDate { get; set; }

        [Comment("削除日時")]
        public DateTime? DeleteDate { get; set; }

        public virtual required Icon Icon { get; set; } = null!;
    }
}

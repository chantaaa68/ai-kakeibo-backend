using AiKakeiboBackend.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.Models
{
    public class Icon : KakeiboInterface
    {
        [Key]
        [Required]
        [Comment("ID")]
        public int Id { get; set; }

        [Required]
        [Comment("アイコン名")]
        public string OfficialIconName { get; set; } = null!;

        [Required]
        [Comment("表示名")]
        public string DefaultIconName { get; set; } = null!;

        [Required]
        [Comment("登録日時")]
        public DateTime CreateDate { get; set; }

        [Required]
        [Comment("更新日時")]
        public DateTime UpdateDate { get; set; }

        [Comment("削除日時")]
        public DateTime? DeleteDate { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<CategoryDefault> CategoryDefaults { get; set; } = new List<CategoryDefault>();
    }
}

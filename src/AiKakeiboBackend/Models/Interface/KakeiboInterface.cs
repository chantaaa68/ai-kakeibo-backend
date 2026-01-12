using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.Models.Interface
{
    public interface KakeiboInterface
    {
        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime UpdateDate { get; set; }

        public DateTime? DeleteDate { get; set; }
    }
}

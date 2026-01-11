using System.ComponentModel.DataAnnotations;

namespace AiKakeiboBackend.DTOs
{
    // Newsletter リクエストDTO

    public class RegistNewsletterRequest
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? MailBody { get; set; }
    }

    public class UpdateNewsletterRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? MailBody { get; set; }
    }

    public class SendNewsletterRequest
    {
        [Required]
        public int NewsletterId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ItemId { get; set; }
    }
}

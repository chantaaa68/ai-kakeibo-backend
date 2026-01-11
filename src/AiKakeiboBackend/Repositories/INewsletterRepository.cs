using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.Repositories
{
    public interface INewsletterRepository
    {
        Task<NewsletterTemplate?> GetByIdAsync(int id);
        Task CreateNewsletterAsync(NewsletterTemplate newsletter);
        Task UpdateNewsletterAsync(NewsletterTemplate newsletter);
        Task<Users?> GetUserByIdAsync(int userId);
        Task<KakeiboItem?> GetItemByIdAsync(int itemId);
    }
}

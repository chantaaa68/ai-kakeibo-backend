using AiKakeiboBackend.Data;
using AiKakeiboBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    public class NewsletterRepository : INewsletterRepository
    {
        private readonly KakeiboDbContext _context;

        public NewsletterRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        public async Task<NewsletterTemplate?> GetByIdAsync(int id)
        {
            return await _context.NewsletterTemplates
                .FirstOrDefaultAsync(n => n.Id == id && n.DeleteDate == null);
        }

        public async Task CreateNewsletterAsync(NewsletterTemplate newsletter)
        {
            newsletter.CreateDate = DateTime.UtcNow;
            newsletter.UpdateDate = DateTime.UtcNow;
            _context.NewsletterTemplates.Add(newsletter);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNewsletterAsync(NewsletterTemplate newsletter)
        {
            newsletter.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<Users?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.DeleteDate == null);
        }

        public async Task<KakeiboItem?> GetItemByIdAsync(int itemId)
        {
            return await _context.KakeiboItems
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.DeleteDate == null);
        }
    }
}

using AiKakeiboBackend.Data;
using AiKakeiboBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    public class KakeiboRepository : IKakeiboRepository
    {
        private readonly KakeiboDbContext _context;

        public KakeiboRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        public async Task<int?> GetKakeiboIdAsync(int userId)
        {
            var kakeibo = await _context.Kakeibos
                .FirstOrDefaultAsync(k => k.UserId == userId && k.DeleteDate == null);
            return kakeibo?.Id;
        }

        public async Task<Kakeibo?> GetByIdAsync(int kakeiboId)
        {
            return await _context.Kakeibos
                .FirstOrDefaultAsync(k => k.Id == kakeiboId && k.DeleteDate == null);
        }

        public async Task<List<KakeiboItem>> GetItemsByKakeiboIdAndRangeAsync(int kakeiboId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.KakeiboItems
                .Include(i => i.Category)
                .Include(i => i.KakeiboItemFrequency)
                .Where(i => i.KakeiboId == kakeiboId && i.DeleteDate == null);

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(i => i.UsedDate >= startDate.Value && i.UsedDate < endDate.Value);
            }

            return await query.OrderByDescending(i => i.UsedDate).ToListAsync();
        }

        public async Task<List<KakeiboItem>> GetItemsByKakeiboIdForMonthAsync(int kakeiboId, DateTime startOfMonth, DateTime endOfMonth)
        {
            return await _context.KakeiboItems
                .Include(i => i.Category)
                .Where(i => i.KakeiboId == kakeiboId
                            && i.DeleteDate == null
                            && i.UsedDate >= startOfMonth
                            && i.UsedDate < endOfMonth)
                .ToListAsync();
        }

        public async Task<KakeiboItem?> GetItemByIdAsync(int itemId)
        {
            return await _context.KakeiboItems
                .Include(i => i.Category)
                .Include(i => i.KakeiboItemFrequency)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.DeleteDate == null);
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.DeleteDate == null);
        }

        public async Task CreateItemAsync(KakeiboItem item)
        {
            item.CreateDate = DateTime.UtcNow;
            item.UpdateDate = DateTime.UtcNow;
            _context.KakeiboItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task CreateFrequencyAsync(KakeiboItemFrequency frequency)
        {
            frequency.CreateDate = DateTime.UtcNow;
            frequency.UpdateDate = DateTime.UtcNow;
            _context.KakeiboItemFrequencies.Add(frequency);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(KakeiboItem item)
        {
            item.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int itemId)
        {
            var item = await _context.KakeiboItems.FindAsync(itemId);
            if (item != null)
            {
                item.DeleteDate = DateTime.UtcNow;
                item.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}

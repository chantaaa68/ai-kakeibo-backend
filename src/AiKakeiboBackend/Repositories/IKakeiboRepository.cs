using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.Repositories
{
    public interface IKakeiboRepository
    {
        Task<int?> GetKakeiboIdAsync(int userId);
        Task<Kakeibo?> GetByIdAsync(int kakeiboId);
        Task<List<KakeiboItem>> GetItemsByKakeiboIdAndRangeAsync(int kakeiboId, DateTime? startDate, DateTime? endDate);
        Task<List<KakeiboItem>> GetItemsByKakeiboIdForMonthAsync(int kakeiboId, DateTime startOfMonth, DateTime endOfMonth);
        Task<KakeiboItem?> GetItemByIdAsync(int itemId);
        Task<Category?> GetCategoryByIdAsync(int categoryId);
        Task CreateItemAsync(KakeiboItem item);
        Task CreateFrequencyAsync(KakeiboItemFrequency frequency);
        Task UpdateItemAsync(KakeiboItem item);
        Task DeleteItemAsync(int itemId);
    }
}

using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.Data;
using AiKakeiboBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// 家計簿および家計簿項目関連のデータアクセスを行うリポジトリクラス
    /// </summary>
    [Repository]
    public class KakeiboRepository : IKakeiboRepository
    {
        private readonly KakeiboDbContext _context;

        /// <summary>
        /// KakeiboRepositoryクラスのコンストラクター
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        public KakeiboRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// ユーザーIDに紐づく家計簿IDを取得します
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <returns>家計簿ID（存在しない場合はnull）</returns>
        public async Task<int?> GetKakeiboIdAsync(int userId)
        {
            var kakeibo = await _context.Kakeibos
                .FirstOrDefaultAsync(k => k.UserId == userId && k.DeleteDate == null);
            return kakeibo?.Id;
        }

        /// <summary>
        /// 家計簿IDを使用して家計簿を取得します
        /// </summary>
        /// <param name="kakeiboId">家計簿ID</param>
        /// <returns>該当する家計簿情報（存在しない場合はnull）</returns>
        public async Task<Kakeibo?> GetByIdAsync(int kakeiboId)
        {
            return await _context.Kakeibos
                .FirstOrDefaultAsync(k => k.Id == kakeiboId && k.DeleteDate == null);
        }

        /// <summary>
        /// 家計簿IDと期間を指定して家計簿項目一覧を取得します
        /// </summary>
        /// <param name="kakeiboId">家計簿ID</param>
        /// <param name="startDate">開始日（null可）</param>
        /// <param name="endDate">終了日（null可）</param>
        /// <returns>家計簿項目のリスト</returns>
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

        /// <summary>
        /// 家計簿IDと月を指定して家計簿項目一覧を取得します
        /// </summary>
        /// <param name="kakeiboId">家計簿ID</param>
        /// <param name="startOfMonth">月初日</param>
        /// <param name="endOfMonth">月末日</param>
        /// <returns>家計簿項目のリスト</returns>
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

        /// <summary>
        /// 項目IDを使用して家計簿項目を取得します
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <returns>該当する家計簿項目情報（存在しない場合はnull）</returns>
        public async Task<KakeiboItem?> GetItemByIdAsync(int itemId)
        {
            return await _context.KakeiboItems
                .Include(i => i.Category)
                .Include(i => i.KakeiboItemFrequency)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.DeleteDate == null);
        }

        /// <summary>
        /// カテゴリIDを使用してカテゴリを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns>該当するカテゴリ情報（存在しない場合はnull）</returns>
        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.DeleteDate == null);
        }

        /// <summary>
        /// 新しい家計簿項目を作成します
        /// </summary>
        /// <param name="item">作成する家計簿項目情報</param>
        /// <returns>非同期処理タスク</returns>
        public async Task CreateItemAsync(KakeiboItem item)
        {
            item.CreateDate = DateTime.UtcNow;
            item.UpdateDate = DateTime.UtcNow;
            _context.KakeiboItems.Add(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 新しい家計簿項目の頻度情報を作成します
        /// </summary>
        /// <param name="frequency">作成する頻度情報</param>
        /// <returns>非同期処理タスク</returns>
        public async Task CreateFrequencyAsync(KakeiboItemFrequency frequency)
        {
            frequency.CreateDate = DateTime.UtcNow;
            frequency.UpdateDate = DateTime.UtcNow;
            _context.KakeiboItemFrequencies.Add(frequency);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 家計簿項目情報を更新します
        /// </summary>
        /// <param name="item">更新する家計簿項目情報</param>
        /// <returns>非同期処理タスク</returns>
        public async Task UpdateItemAsync(KakeiboItem item)
        {
            item.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 家計簿項目を論理削除します
        /// </summary>
        /// <param name="itemId">削除する項目ID</param>
        /// <returns>非同期処理タスク</returns>
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

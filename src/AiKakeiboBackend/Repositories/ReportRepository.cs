using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.Data;
using AiKakeiboBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// レポート関連のデータアクセスを行うリポジトリクラス
    /// </summary>
    [Repository]
    public class ReportRepository : IReportRepository
    {
        private readonly KakeiboDbContext _context;

        /// <summary>
        /// ReportRepositoryクラスのコンストラクター
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        public ReportRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// カテゴリIDに紐づく全期間の取引データを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns>取引データのリスト</returns>
        public async Task<List<KakeiboItem>> GetItemsByCategoryIdAsync(int categoryId)
        {
            return await _context.KakeiboItems
                .Include(i => i.Category)
                .Where(i => i.CategoryId == categoryId && i.DeleteDate == null)
                .OrderBy(i => i.UsedDate)
                .ToListAsync();
        }

        /// <summary>
        /// カテゴリIDと期間を指定して取引データを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <param name="startDate">開始日</param>
        /// <param name="endDate">終了日</param>
        /// <returns>取引データのリスト</returns>
        public async Task<List<KakeiboItem>> GetItemsByCategoryIdAndRangeAsync(int categoryId, DateTime startDate, DateTime endDate)
        {
            return await _context.KakeiboItems
                .Include(i => i.Category)
                .Where(i => i.CategoryId == categoryId
                            && i.DeleteDate == null
                            && i.UsedDate >= startDate
                            && i.UsedDate < endDate)
                .OrderBy(i => i.UsedDate)
                .ToListAsync();
        }

        /// <summary>
        /// カテゴリIDを使用してカテゴリを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns>該当するカテゴリ情報（存在しない場合はnull）</returns>
        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories
                .Include(c => c.Icon)
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.DeleteDate == null);
        }
    }
}

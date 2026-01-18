using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
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
            Kakeibo? kakeibo = await _context.Kakeibo
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
            return await _context.Kakeibo
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
            IQueryable<KakeiboItem> query = _context.KakeiboItem
                .Where(i => i.KakeiboId == kakeiboId && i.DeleteDate == null)
                .Include(i => i.Category)
                    .ThenInclude(i => i.Icon)
                .Include(i => i.KakeiboItemFrequency);

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
            return await _context.KakeiboItem
                .Where(i => i.KakeiboId == kakeiboId
                            && i.DeleteDate == null
                            && i.UsedDate >= startOfMonth
                            && i.UsedDate < endOfMonth)
                .Include(i => i.Category)
                    .ThenInclude(i => i.Icon)
                .ToListAsync();
        }

        /// <summary>
        /// 項目IDを使用して家計簿項目を取得します
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <returns>該当する家計簿項目情報（存在しない場合はnull）</returns>
        public async Task<KakeiboItem?> GetItemByIdAsync(int itemId)
        {
            return await _context.KakeiboItem
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
            return await _context.Category
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
            _context.KakeiboItem.Add(item);
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
            _context.KakeiboItemFrequency.Add(frequency);
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
            KakeiboItem? item = await _context.KakeiboItem.FindAsync(itemId);
            if (item != null)
            {
                item.DeleteDate = DateTime.UtcNow;
                item.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 全期間の月次集計データを取得します（最適化版）
        /// パフォーマンス最適化：
        /// 1. AsNoTracking() でトラッキングを無効化（読み取り専用クエリの高速化）
        /// 2. Select() で必要なカラムのみを射影（データ転送量削減）
        /// 3. 匿名型で中間データを取得し、メモリ上で効率的にグルーピング
        /// </summary>
        /// <param name="kakeiboId">家計簿ID</param>
        /// <returns>月次レポートリスト</returns>
        public async Task<List<MonthlyReport>> GetMonthlyReportDataAsync(int kakeiboId)
        {
            // 必要なデータのみを射影して取得（最適化ポイント1: AsNoTracking + Select）
            var itemData = await _context.KakeiboItem
                .AsNoTracking()
                .Where(k => k.KakeiboId == kakeiboId && k.DeleteDate == null)
                .Select(k => new
                {
                    k.CategoryId,
                    k.InoutFlg,
                    Year = k.UsedDate.Year,
                    Month = k.UsedDate.Month,
                    CategoryName = k.Category.CategoryName,
                    IconName = k.Category.Icon.OfficialIconName ?? string.Empty,
                    k.ItemAmount
                })
                .ToListAsync();

            // メモリ上でグルーピング（最適化ポイント2: 単一のデータ取得後に効率的にグルーピング）
            var monthlyReports = itemData
                .GroupBy(k => k.InoutFlg)
                .Select(inoutGroup => new MonthlyReport
                {
                    InoutFlg = inoutGroup.Key,
                    MonthlyReportItems = inoutGroup
                        .GroupBy(e => new { e.Year, e.Month })
                        .Select(monthGroup => new MonthlyReportItem
                        {
                            UsedMonth = $"{monthGroup.Key.Year:D4}-{monthGroup.Key.Month:D2}",
                            CategoryReportItems = monthGroup
                                .GroupBy(t => new { t.CategoryId,t.CategoryName, t.IconName })
                                .Select(categoryGroup => new CategoryReportItem
                                {
                                    CategoryId = categoryGroup.Key.CategoryId,
                                    CategoryName = categoryGroup.Key.CategoryName,
                                    IconName = categoryGroup.Key.IconName,
                                    TotalAmount = categoryGroup.Sum(e => e.ItemAmount)
                                })
                                .OrderByDescending(c => c.TotalAmount)
                                .ToList()
                        })
                        .OrderByDescending(m => m.UsedMonth)
                        .ToList()
                })
                .ToList();

            return monthlyReports;
        }
    }
}

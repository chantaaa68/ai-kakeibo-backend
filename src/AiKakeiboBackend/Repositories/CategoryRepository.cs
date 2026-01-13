using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// カテゴリ関連のデータアクセスを行うリポジトリクラス
    /// </summary>
    [Repository]
    public class CategoryRepository : ICategoryRepository
    {
        private readonly KakeiboDbContext _context;

        /// <summary>
        /// CategoryRepositoryクラスのコンストラクター
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        public CategoryRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 家計簿IDに紐づくカテゴリ一覧を取得します
        /// </summary>
        /// <param name="kakeiboId">家計簿ID</param>
        /// <returns>カテゴリ情報のリスト</returns>
        public async Task<List<Category>> GetCategoriesByKakeiboIdAsync(int kakeiboId)
        {
            return await _context.Category
                .Include(c => c.Icon)
                .Where(c => c.KakeiboID == kakeiboId && c.DeleteDate == null)
                .ToListAsync();
        }

        /// <summary>
        /// デフォルトカテゴリ一覧を取得します
        /// </summary>
        /// <returns>デフォルトカテゴリ情報のリスト</returns>
        public async Task<List<CategoryDefault>> GetDefaultCategoriesAsync()
        {
            return await _context.CategoryDefault
                .Include(c => c.Icon)
                .Where(c => c.DeleteDate == null)
                .ToListAsync();
        }

        /// <summary>
        /// カテゴリIDを使用してカテゴリを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns>該当するカテゴリ情報（存在しない場合はnull）</returns>
        public async Task<Category?> GetByIdAsync(int categoryId)
        {
            return await _context.Category
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.DeleteDate == null);
        }

        /// <summary>
        /// アイコン名を使用してアイコンを取得します
        /// </summary>
        /// <param name="iconName">アイコン名</param>
        /// <returns>該当するアイコン情報（存在しない場合はnull）</returns>
        public async Task<Icon?> GetIconByNameAsync(string iconName)
        {
            return await _context.Icon
                .FirstOrDefaultAsync(i => i.OfficialIconName == iconName && i.DeleteDate == null);
        }

        /// <summary>
        /// 新しいカテゴリを作成します
        /// </summary>
        /// <param name="category">作成するカテゴリ情報</param>
        /// <returns>非同期処理タスク</returns>
        public async Task CreateCategoryAsync(Category category)
        {
            category.CreateDate = DateTime.UtcNow;
            category.UpdateDate = DateTime.UtcNow;
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// カテゴリ情報を更新します
        /// </summary>
        /// <param name="category">更新するカテゴリ情報</param>
        /// <returns>非同期処理タスク</returns>
        public async Task UpdateCategoryAsync(Category category)
        {
            category.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}

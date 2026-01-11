using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.Data;
using AiKakeiboBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// ニュースレター関連のデータアクセスを行うリポジトリクラス
    /// </summary>
    [Repository]
    public class NewsletterRepository : INewsletterRepository
    {
        private readonly KakeiboDbContext _context;

        /// <summary>
        /// NewsletterRepositoryクラスのコンストラクター
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        public NewsletterRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// ニュースレターテンプレートIDを使用してニュースレターテンプレートを取得します
        /// </summary>
        /// <param name="id">ニュースレターテンプレートID</param>
        /// <returns>該当するニュースレターテンプレート情報（存在しない場合はnull）</returns>
        public async Task<NewsletterTemplate?> GetByIdAsync(int id)
        {
            return await _context.NewsletterTemplates
                .FirstOrDefaultAsync(n => n.Id == id && n.DeleteDate == null);
        }

        /// <summary>
        /// 新しいニュースレターテンプレートを作成します
        /// </summary>
        /// <param name="newsletter">作成するニュースレターテンプレート情報</param>
        /// <returns>非同期処理タスク</returns>
        public async Task CreateNewsletterAsync(NewsletterTemplate newsletter)
        {
            newsletter.CreateDate = DateTime.UtcNow;
            newsletter.UpdateDate = DateTime.UtcNow;
            _context.NewsletterTemplates.Add(newsletter);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// ニュースレターテンプレート情報を更新します
        /// </summary>
        /// <param name="newsletter">更新するニュースレターテンプレート情報</param>
        /// <returns>非同期処理タスク</returns>
        public async Task UpdateNewsletterAsync(NewsletterTemplate newsletter)
        {
            newsletter.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// ユーザーIDを使用してユーザーを取得します
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <returns>該当するユーザー情報（存在しない場合はnull）</returns>
        public async Task<Users?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.DeleteDate == null);
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
                .FirstOrDefaultAsync(i => i.Id == itemId && i.DeleteDate == null);
        }
    }
}

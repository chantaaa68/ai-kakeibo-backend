using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.Data;
using AiKakeiboBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// ユーザー関連のデータアクセスを行うリポジトリクラス
    /// </summary>
    [Repository]
    public class UserRepository : IUserRepository
    {
        private readonly KakeiboDbContext _context;

        /// <summary>
        /// UserRepositoryクラスのコンストラクター
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        public UserRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// メールアドレスとユーザーハッシュ値を使用してユーザーを取得します
        /// </summary>
        /// <param name="email">メールアドレス</param>
        /// <param name="userHash">ユーザーハッシュ値</param>
        /// <returns>該当するユーザー情報（存在しない場合はnull）</returns>
        public async Task<Users?> GetByEmailAndHashAsync(string email, string userHash)
        {
            return await _context.Users
                .Include(u => u.Kakeibos.Where(k => k.DeleteDate == null))
                .FirstOrDefaultAsync(u => u.Email == email
                                          && u.UserHash == userHash
                                          && u.DeleteDate == null);
        }

        /// <summary>
        /// ユーザーIDを使用してユーザーを取得します
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <returns>該当するユーザー情報（存在しない場合はnull）</returns>
        public async Task<Users?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Kakeibos.Where(k => k.DeleteDate == null))
                .FirstOrDefaultAsync(u => u.Id == userId && u.DeleteDate == null);
        }

        /// <summary>
        /// メールアドレスを使用してユーザーを取得します
        /// </summary>
        /// <param name="email">メールアドレス</param>
        /// <returns>該当するユーザー情報（存在しない場合はnull）</returns>
        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.DeleteDate == null);
        }

        /// <summary>
        /// 新しいユーザーを作成します
        /// </summary>
        /// <param name="user">作成するユーザー情報</param>
        /// <returns>作成されたユーザー情報</returns>
        public async Task<Users> CreateUserAsync(Users user)
        {
            user.CreateDate = DateTime.UtcNow;
            user.UpdateDate = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// ユーザー情報を更新します
        /// </summary>
        /// <param name="user">更新するユーザー情報</param>
        /// <returns>非同期処理タスク</returns>
        public async Task UpdateUserAsync(Users user)
        {
            user.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// ユーザーを論理削除します
        /// </summary>
        /// <param name="userId">削除するユーザーID</param>
        /// <returns>非同期処理タスク</returns>
        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.DeleteDate = DateTime.UtcNow;
                user.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// ユーザーIDに紐づく家計簿を取得します
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <returns>該当する家計簿情報（存在しない場合はnull）</returns>
        public async Task<Kakeibo?> GetKakeiboByUserIdAsync(int userId)
        {
            return await _context.Kakeibos
                .FirstOrDefaultAsync(k => k.UserId == userId && k.DeleteDate == null);
        }

        /// <summary>
        /// 新しい家計簿を作成します
        /// </summary>
        /// <param name="kakeibo">作成する家計簿情報</param>
        /// <returns>作成された家計簿情報</returns>
        public async Task<Kakeibo> CreateKakeiboAsync(Kakeibo kakeibo)
        {
            kakeibo.CreateDate = DateTime.UtcNow;
            kakeibo.UpdateDate = DateTime.UtcNow;
            _context.Kakeibos.Add(kakeibo);
            await _context.SaveChangesAsync();
            return kakeibo;
        }

        /// <summary>
        /// 家計簿情報を更新します
        /// </summary>
        /// <param name="kakeibo">更新する家計簿情報</param>
        /// <returns>非同期処理タスク</returns>
        public async Task UpdateKakeiboAsync(Kakeibo kakeibo)
        {
            kakeibo.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}

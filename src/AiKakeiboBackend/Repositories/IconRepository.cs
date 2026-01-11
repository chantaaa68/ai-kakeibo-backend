using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// アイコン関連のデータアクセスを行うリポジトリクラス
    /// </summary>
    [Repository]
    public class IconRepository : IIconRepository
    {
        private readonly KakeiboDbContext _context;

        /// <summary>
        /// IconRepositoryクラスのコンストラクター
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        public IconRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// すべてのアイコン情報を取得します
        /// </summary>
        /// <returns>アイコン情報のリスト</returns>
        public async Task<List<IconDto>> GetAllIconsAsync()
        {
            return await _context.Icons
                .Where(i => i.DeleteDate == null)
                .Select(i => new IconDto
                {
                    Id = i.Id,
                    IconName = i.DefaultIconName,
                    IconPath = i.OfficialIconName
                })
                .ToListAsync();
        }
    }
}

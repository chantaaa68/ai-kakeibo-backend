using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// アイコン関連のデータアクセスを行うリポジトリインターフェース
    /// </summary>
    public interface IIconRepository
    {
        /// <summary>
        /// すべてのアイコン情報を取得します
        /// </summary>
        /// <returns>アイコン情報のリスト</returns>
        Task<List<Icon>> GetAllIconsAsync();
    }
}

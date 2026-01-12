using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// ニュースレター関連のデータアクセスを行うリポジトリインターフェース
    /// </summary>
    public interface INewsletterRepository
    {
        /// <summary>
        /// ニュースレターテンプレートIDを使用してニュースレターテンプレートを取得します
        /// </summary>
        /// <param name="id">ニュースレターテンプレートID</param>
        /// <returns>該当するニュースレターテンプレート情報（存在しない場合はnull）</returns>
        Task<NewsletterTemplate?> GetByIdAsync(int id);

        /// <summary>
        /// 新しいニュースレターテンプレートを作成します
        /// </summary>
        /// <param name="newsletter">作成するニュースレターテンプレート情報</param>
        /// <returns>非同期処理タスク</returns>
        Task CreateNewsletterAsync(NewsletterTemplate newsletter);

        /// <summary>
        /// ニュースレターテンプレート情報を更新します
        /// </summary>
        /// <param name="newsletter">更新するニュースレターテンプレート情報</param>
        /// <returns>非同期処理タスク</returns>
        Task UpdateNewsletterAsync(NewsletterTemplate newsletter);

        /// <summary>
        /// ユーザーIDを使用してユーザーを取得します
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <returns>該当するユーザー情報（存在しない場合はnull）</returns>
        Task<Users?> GetUserByIdAsync(int userId);

        /// <summary>
        /// 項目IDを使用して家計簿項目を取得します
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <returns>該当する家計簿項目情報（存在しない場合はnull）</returns>
        Task<KakeiboItem?> GetItemByIdAsync(int itemId);
    }
}

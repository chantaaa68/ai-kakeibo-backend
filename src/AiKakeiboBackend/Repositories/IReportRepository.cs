using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// レポート関連のデータアクセスを行うリポジトリインターフェース
    /// </summary>
    public interface IReportRepository
    {
        /// <summary>
        /// カテゴリIDに紐づく全期間の取引データを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns>取引データのリスト</returns>
        Task<List<KakeiboItem>> GetItemsByCategoryIdAsync(int categoryId);

        /// <summary>
        /// カテゴリIDと期間を指定して取引データを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <param name="startDate">開始日</param>
        /// <param name="endDate">終了日</param>
        /// <returns>取引データのリスト</returns>
        Task<List<KakeiboItem>> GetItemsByCategoryIdAndRangeAsync(int categoryId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// カテゴリIDを使用してカテゴリを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns>該当するカテゴリ情報（存在しない場合はnull）</returns>
        Task<Category?> GetCategoryByIdAsync(int categoryId);
    }
}

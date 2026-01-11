using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// カテゴリ関連のデータアクセスを行うリポジトリインターフェース
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// 家計簿IDに紐づくカテゴリ一覧を取得します
        /// </summary>
        /// <param name="kakeiboId">家計簿ID</param>
        /// <returns>カテゴリ情報のリスト</returns>
        Task<List<Category>> GetCategoriesByKakeiboIdAsync(int kakeiboId);

        /// <summary>
        /// デフォルトカテゴリ一覧を取得します
        /// </summary>
        /// <returns>デフォルトカテゴリ情報のリスト</returns>
        Task<List<CategoryDefault>> GetDefaultCategoriesAsync();

        /// <summary>
        /// カテゴリIDを使用してカテゴリを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns>該当するカテゴリ情報（存在しない場合はnull）</returns>
        Task<Category?> GetByIdAsync(int categoryId);

        /// <summary>
        /// アイコン名を使用してアイコンを取得します
        /// </summary>
        /// <param name="iconName">アイコン名</param>
        /// <returns>該当するアイコン情報（存在しない場合はnull）</returns>
        Task<Icon?> GetIconByNameAsync(string iconName);

        /// <summary>
        /// 新しいカテゴリを作成します
        /// </summary>
        /// <param name="category">作成するカテゴリ情報</param>
        /// <returns>非同期処理タスク</returns>
        Task CreateCategoryAsync(Category category);

        /// <summary>
        /// カテゴリ情報を更新します
        /// </summary>
        /// <param name="category">更新するカテゴリ情報</param>
        /// <returns>非同期処理タスク</returns>
        Task UpdateCategoryAsync(Category category);
    }
}

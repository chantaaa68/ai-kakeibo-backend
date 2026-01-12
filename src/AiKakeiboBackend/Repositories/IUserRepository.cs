using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// ユーザー関連のデータアクセスを行うリポジトリインターフェース
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// メールアドレスとユーザーハッシュ値を使用してユーザーを取得します
        /// </summary>
        /// <param name="email">メールアドレス</param>
        /// <param name="userHash">ユーザーハッシュ値</param>
        /// <returns>該当するユーザー情報（存在しない場合はnull）</returns>
        Task<Users?> GetByEmailAndHashAsync(string email, string userHash);

        /// <summary>
        /// ユーザーIDを使用してユーザーを取得します
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <returns>該当するユーザー情報（存在しない場合はnull）</returns>
        Task<Users?> GetByIdAsync(int userId);

        /// <summary>
        /// メールアドレスを使用してユーザーを取得します
        /// </summary>
        /// <param name="email">メールアドレス</param>
        /// <returns>該当するユーザー情報（存在しない場合はnull）</returns>
        Task<Users?> GetByEmailAsync(string email);

        /// <summary>
        /// 新しいユーザーを作成します
        /// </summary>
        /// <param name="user">作成するユーザー情報</param>
        /// <returns>作成されたユーザー情報</returns>
        Task<Users> CreateUserAsync(Users user);

        /// <summary>
        /// ユーザー情報を更新します
        /// </summary>
        /// <param name="user">更新するユーザー情報</param>
        /// <returns>非同期処理タスク</returns>
        Task UpdateUserAsync(Users user);

        /// <summary>
        /// ユーザーを論理削除します
        /// </summary>
        /// <param name="userId">削除するユーザーID</param>
        /// <returns>非同期処理タスク</returns>
        Task DeleteUserAsync(int userId);

        /// <summary>
        /// ユーザーIDに紐づく家計簿を取得します
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <returns>該当する家計簿情報（存在しない場合はnull）</returns>
        Task<Kakeibo?> GetKakeiboByUserIdAsync(int userId);

        /// <summary>
        /// 新しい家計簿を作成します
        /// </summary>
        /// <param name="kakeibo">作成する家計簿情報</param>
        /// <returns>作成された家計簿情報</returns>
        Task<Kakeibo> CreateKakeiboAsync(Kakeibo kakeibo);

        /// <summary>
        /// 家計簿情報を更新します
        /// </summary>
        /// <param name="kakeibo">更新する家計簿情報</param>
        /// <returns>非同期処理タスク</returns>
        Task UpdateKakeiboAsync(Kakeibo kakeibo);
    }
}

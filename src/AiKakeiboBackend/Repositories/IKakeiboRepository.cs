using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.Repositories
{
    /// <summary>
    /// 家計簿および家計簿項目関連のデータアクセスを行うリポジトリインターフェース
    /// </summary>
    public interface IKakeiboRepository
    {
        /// <summary>
        /// ユーザーIDに紐づく家計簿IDを取得します
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <returns>家計簿ID（存在しない場合はnull）</returns>
        Task<int?> GetKakeiboIdAsync(int userId);

        /// <summary>
        /// 家計簿IDを使用して家計簿を取得します
        /// </summary>
        /// <param name="kakeiboId">家計簿ID</param>
        /// <returns>該当する家計簿情報（存在しない場合はnull）</returns>
        Task<Kakeibo?> GetByIdAsync(int kakeiboId);

        /// <summary>
        /// 家計簿IDと期間を指定して家計簿項目一覧を取得します
        /// </summary>
        /// <param name="kakeiboId">家計簿ID</param>
        /// <param name="startDate">開始日（null可）</param>
        /// <param name="endDate">終了日（null可）</param>
        /// <returns>家計簿項目のリスト</returns>
        Task<List<KakeiboItem>> GetItemsByKakeiboIdAndRangeAsync(int kakeiboId, DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// 家計簿IDと月を指定して家計簿項目一覧を取得します
        /// </summary>
        /// <param name="kakeiboId">家計簿ID</param>
        /// <param name="startOfMonth">月初日</param>
        /// <param name="endOfMonth">月末日</param>
        /// <returns>家計簿項目のリスト</returns>
        Task<List<KakeiboItem>> GetItemsByKakeiboIdForMonthAsync(int kakeiboId, DateTime startOfMonth, DateTime endOfMonth);

        /// <summary>
        /// 項目IDを使用して家計簿項目を取得します
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <returns>該当する家計簿項目情報（存在しない場合はnull）</returns>
        Task<KakeiboItem?> GetItemByIdAsync(int itemId);

        /// <summary>
        /// カテゴリIDを使用してカテゴリを取得します
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns>該当するカテゴリ情報（存在しない場合はnull）</returns>
        Task<Category?> GetCategoryByIdAsync(int categoryId);

        /// <summary>
        /// 新しい家計簿項目を作成します
        /// </summary>
        /// <param name="item">作成する家計簿項目情報</param>
        /// <returns>非同期処理タスク</returns>
        Task CreateItemAsync(KakeiboItem item);

        /// <summary>
        /// 新しい家計簿項目の頻度情報を作成します
        /// </summary>
        /// <param name="frequency">作成する頻度情報</param>
        /// <returns>非同期処理タスク</returns>
        Task CreateFrequencyAsync(KakeiboItemFrequency frequency);

        /// <summary>
        /// 家計簿項目情報を更新します
        /// </summary>
        /// <param name="item">更新する家計簿項目情報</param>
        /// <returns>非同期処理タスク</returns>
        Task UpdateItemAsync(KakeiboItem item);

        /// <summary>
        /// 家計簿項目を論理削除します
        /// </summary>
        /// <param name="itemId">削除する項目ID</param>
        /// <returns>非同期処理タスク</returns>
        Task DeleteItemAsync(int itemId);

        /// <summary>
        /// 全期間の月次集計データを取得します（最適化版）
        /// </summary>
        /// <param name="kakeiboId">家計簿ID</param>
        /// <returns>月次レポートリスト</returns>
        Task<List<MonthlyReport>> GetMonthlyReportDataAsync(int kakeiboId);
    }
}

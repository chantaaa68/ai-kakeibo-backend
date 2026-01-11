using AiKakeiboBackend.DTOs;

namespace AiKakeiboBackend.Services
{
    /// <summary>
    /// HTTP レスポンスを統一的に生成するサービス
    /// </summary>
    public class HttpResponseService
    {
        /// <summary>
        /// 成功レスポンスを生成する
        /// </summary>
        /// <typeparam name="T">データの型</typeparam>
        /// <param name="data">レスポンスデータ</param>
        /// <param name="message">メッセージ（省略可）</param>
        /// <returns>成功レスポンス</returns>
        public static ApiResponse<T> Ok<T>(T data, string message = "処理が正常に完了しました")
        {
            return new ApiResponse<T>
            {
                Status = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// 失敗レスポンスを生成する
        /// </summary>
        /// <typeparam name="T">データの型</typeparam>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="data">レスポンスデータ（省略可）</param>
        /// <returns>失敗レスポンス</returns>
        public static ApiResponse<T> Fail<T>(string message, T? data = default)
        {
            return new ApiResponse<T>
            {
                Status = false,
                Message = message,
                Data = data
            };
        }
    }
}

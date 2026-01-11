using AiKakeiboBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Helpers
{
    /// <summary>
    /// API レスポンスヘルパー
    /// </summary>
    public static class ApiResponseHelper
    {
        /// <summary>
        /// 成功レスポンスを生成する
        /// </summary>
        /// <typeparam name="T">データの型</typeparam>
        /// <param name="result">レスポンスデータ</param>
        /// <param name="message">メッセージ（省略可）</param>
        /// <returns>成功レスポンス</returns>
        public static IActionResult Success<T>(T result, string? message = null)
        {
            return new OkObjectResult(new ApiResponse<T>
            {
                Status = true,
                Message = message,
                Result = result
            });
        }

        /// <summary>
        /// 失敗レスポンスを生成する
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <returns>失敗レスポンス</returns>
        public static IActionResult Fail(string message)
        {
            return new BadRequestObjectResult(new ApiResponse<object>
            {
                Status = false,
                Message = message,
                Result = default
            });
        }
    }
}

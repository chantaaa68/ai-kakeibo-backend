namespace AiKakeiboBackend.DTOs
{
    /// <summary>
    /// API共通レスポンス形式
    /// </summary>
    /// <typeparam name="T">レスポンスデータの型</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// 処理成功フラグ (true: 成功, false: 失敗)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// エラーメッセージ (失敗時、フロントに表示するための文字列)
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 実際のAPIレスポンス内容
        /// </summary>
        public T? Data { get; set; }
    }
}

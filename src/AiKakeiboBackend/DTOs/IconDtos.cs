namespace AiKakeiboBackend.DTOs
{
    // Icon レスポンスDTO

    /// <summary>
    /// アイコン情報DTO
    /// </summary>
    public class IconDto
    {
        /// <summary>
        /// アイコンID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// アイコン名
        /// </summary>
        public string IconName { get; set; } = string.Empty;
        /// <summary>
        /// アイコンパス
        /// </summary>
        public string IconPath { get; set; } = string.Empty;
    }

    /// <summary>
    /// アイコンリストレスポンス
    /// </summary>
    public class IconListResponse
    {
        /// <summary>
        /// アイコンリスト
        /// </summary>
        public List<IconDto> Icons { get; set; } = new();
    }
}

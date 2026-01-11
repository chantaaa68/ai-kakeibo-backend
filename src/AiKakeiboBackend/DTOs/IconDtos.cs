namespace AiKakeiboBackend.DTOs
{
    // Icon レスポンスDTO

    /// <summary>
    /// アイコンリスト取得レスポンス
    /// </summary>
    public class GetIconListResponse
    {
        /// <summary>
        /// アイコンデータリスト
        /// </summary>
        public List<IconData> IconDatas { get; set; } = new();
    }

    /// <summary>
    /// アイコンデータ
    /// </summary>
    public class IconData
    {
        /// <summary>
        /// アイコンID
        /// </summary>
        public int IconId { get; set; }
        /// <summary>
        /// 公式アイコン名
        /// </summary>
        public string OfficialIconName { get; set; } = string.Empty;
        /// <summary>
        /// デフォルトアイコン名
        /// </summary>
        public string DefaultIconName { get; set; } = string.Empty;
    }
}

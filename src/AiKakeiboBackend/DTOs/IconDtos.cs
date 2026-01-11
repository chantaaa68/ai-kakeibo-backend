namespace AiKakeiboBackend.DTOs
{
    // Icon レスポンスDTO

    public class IconDto
    {
        public int Id { get; set; }
        public string IconName { get; set; } = string.Empty;
        public string IconPath { get; set; } = string.Empty;
    }

    public class IconListResponse
    {
        public List<IconDto> Icons { get; set; } = new();
    }
}

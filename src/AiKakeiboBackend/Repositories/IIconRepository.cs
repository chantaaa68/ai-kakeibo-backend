using AiKakeiboBackend.DTOs;

namespace AiKakeiboBackend.Repositories
{
    public interface IIconRepository
    {
        Task<List<IconDto>> GetAllIconsAsync();
    }
}

using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Repositories
{
    public class IconRepository : IIconRepository
    {
        private readonly KakeiboDbContext _context;

        public IconRepository(KakeiboDbContext context)
        {
            _context = context;
        }

        public async Task<List<IconDto>> GetAllIconsAsync()
        {
            return await _context.Icons
                .Where(i => i.DeleteDate == null)
                .Select(i => new IconDto
                {
                    Id = i.Id,
                    IconName = i.DefaultIconName,
                    IconPath = i.OfficialIconName
                })
                .ToListAsync();
        }
    }
}

using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public class IconService : IIconService
    {
        private readonly IIconRepository _iconRepository;

        public IconService(IIconRepository iconRepository)
        {
            _iconRepository = iconRepository;
        }

        public async Task<IActionResult> GetIconListAsync()
        {
            try
            {
                var icons = await _iconRepository.GetAllIconsAsync();

                var response = new IconListResponse
                {
                    Icons = icons
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイコン一覧取得中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}

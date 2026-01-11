using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    [Service]
    public class IconService : IIconService
    {
        private readonly IIconRepository _iconRepository;

        public IconService(IIconRepository iconRepository)
        {
            _iconRepository = iconRepository;
        }

        /// <summary>
        /// システムに登録されている全てのアイコン情報を取得します。
        /// カテゴリ作成時や更新時にアイコンを選択するためのマスタデータとして利用されます。
        /// </summary>
        /// <returns>アイコン一覧を含むApiResponse。取得成功時はアイコン情報の配列、失敗時はエラーメッセージを返却</returns>
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

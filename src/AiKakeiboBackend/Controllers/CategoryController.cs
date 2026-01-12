using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// カテゴリ情報取得APIです。指定された条件に基づいてカテゴリ情報を取得します。
        /// ユーザーIDを指定した場合はそのユーザーのカスタムカテゴリを、DefaultFlgを指定した場合はデフォルトカテゴリを取得します。
        /// </summary>
        /// <param name="req">カテゴリデータ取得リクエスト（UserId, DefaultFlg）</param>
        /// <returns>カテゴリ情報のリスト</returns>
        [HttpPost("GetCategoryData")]
        public async Task<IActionResult> GetCategoryDataAsync([FromBody] GetCategoryDataRequest req)
        {
            return await _service.GetCategoryDataAsync(req);
        }

        /// <summary>
        /// カテゴリ登録APIです。新規カテゴリを登録します。
        /// ユーザー独自のカスタムカテゴリを作成する際に使用します。
        /// </summary>
        /// <param name="req">カテゴリ登録リクエスト（CategoryName, IconId, CategoryType等）</param>
        /// <returns>登録結果（登録されたカテゴリ情報）</returns>
        [HttpPost("RegistCategory")]
        public async Task<IActionResult> RegistCategoryAsync([FromBody] RegistCategoryRequest req)
        {
            return await _service.RegistCategoryAsync(req);
        }

        /// <summary>
        /// カテゴリ更新APIです。指定されたカテゴリIDのカテゴリ情報（カテゴリ名、アイコン等）を更新します。
        /// </summary>
        /// <param name="req">カテゴリ更新リクエスト（CategoryId, CategoryName, IconId等）</param>
        /// <returns>更新結果</returns>
        [HttpPost("UpdateCategory")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] UpdateCategoryRequest req)
        {
            return await _service.UpdateCategoryAsync(req);
        }
    }
}

using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// ユーザーログインAPIです。メールアドレスとパスワードハッシュでユーザー認証を行い、
        /// 成功時にはJWTトークンとユーザー情報を返却します。
        /// </summary>
        /// <param name="req">ログインリクエスト（Email, UserHash）</param>
        /// <returns>ログイン結果（トークン、ユーザー情報、家計簿ID）</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest req)
        {
            return await _service.LoginAsync(req);
        }

        /// <summary>
        /// ユーザー情報取得APIです。指定されたユーザーIDに紐づくユーザー情報（名前、メールアドレス等）を取得します。
        /// </summary>
        /// <param name="req">ユーザーデータ取得リクエスト（UserId）</param>
        /// <returns>ユーザー情報</returns>
        [HttpPost("GetUserData")]
        public async Task<IActionResult> GetUserDataAsync([FromBody] GetUserDataRequest req)
        {
            return await _service.GetUserDataAsync(req);
        }

        /// <summary>
        /// ユーザー登録APIです。新規ユーザーを登録し、家計簿情報も自動生成します。
        /// メールアドレスの重複チェックを行い、既に登録済みの場合はエラーを返却します。
        /// </summary>
        /// <param name="req">ユーザー登録リクエスト（Email, UserHash, UserName等）</param>
        /// <returns>登録結果（登録されたユーザー情報）</returns>
        [HttpPost("Regist")]
        public async Task<IActionResult> RegistAsync([FromBody] RegistUserRequest req)
        {
            return await _service.RegistAsync(req);
        }

        /// <summary>
        /// ユーザー情報更新APIです。指定されたユーザーIDのユーザー情報（名前、メールアドレス、パスワードハッシュ等）を更新します。
        /// </summary>
        /// <param name="req">ユーザー更新リクエスト（UserId, Email, UserHash, UserName等）</param>
        /// <returns>更新結果</returns>
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserRequest req)
        {
            return await _service.UpdateAsync(req);
        }

        /// <summary>
        /// ユーザー削除APIです。指定されたユーザーIDのユーザー情報を論理削除（DeleteFlgを立てる）します。
        /// 関連する家計簿データも併せて論理削除されます。
        /// </summary>
        /// <param name="req">ユーザー削除リクエスト（UserId）</param>
        /// <returns>削除結果</returns>
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteUserRequest req)
        {
            return await _service.DeleteAsync(req);
        }
    }
}

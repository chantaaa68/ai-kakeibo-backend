using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly KakeiboDbContext _context;
        private readonly JwtService _jwtService;

        public UserController(KakeiboDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        /// <summary>
        /// ユーザーログイン
        /// </summary>
        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            try
            {
                // ユーザー検索 (メールアドレスとハッシュ値で認証)
                var user = await _context.Users
                    .Include(u => u.Kakeibos.Where(k => k.DeleteDate == null))
                    .FirstOrDefaultAsync(u => u.Email == request.Email
                                              && u.UserHash == request.UserHash
                                              && u.DeleteDate == null);

                if (user == null)
                {
                    return Ok(HttpResponseService.Fail<LoginResponse>("メールアドレスまたはパスワードが正しくありません"));
                }

                // 最初のKakeiboを取得
                var kakeibo = user.Kakeibos.FirstOrDefault();
                if (kakeibo == null)
                {
                    return Ok(HttpResponseService.Fail<LoginResponse>("家計簿が見つかりません"));
                }

                // JWTトークンを生成
                var token = _jwtService.GenerateToken(user.Id, user.Email);

                var response = new LoginResponse
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    Email = user.Email,
                    Token = token,
                    KakeiboId = kakeibo.Id,
                    KakeiboName = kakeibo.KakeiboName
                };

                return Ok(HttpResponseService.Ok(response, "ログインに成功しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<LoginResponse>($"ログイン処理中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// ユーザー情報取得
        /// </summary>
        [HttpGet("GetUserData")]
        public async Task<ActionResult<ApiResponse<UserDataResponse>>> GetUserData([FromQuery] int UserId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Kakeibos.Where(k => k.DeleteDate == null))
                    .FirstOrDefaultAsync(u => u.Id == UserId && u.DeleteDate == null);

                if (user == null)
                {
                    return Ok(HttpResponseService.Fail<UserDataResponse>("ユーザーが見つかりません"));
                }

                var kakeibo = user.Kakeibos.FirstOrDefault();
                if (kakeibo == null)
                {
                    return Ok(HttpResponseService.Fail<UserDataResponse>("家計簿が見つかりません"));
                }

                var response = new UserDataResponse
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    Email = user.Email,
                    KakeiboId = kakeibo.Id,
                    KakeiboName = kakeibo.KakeiboName,
                    KakeiboExplanation = kakeibo.KakeiboExplanation ?? string.Empty
                };

                return Ok(HttpResponseService.Ok(response));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<UserDataResponse>($"ユーザー情報取得中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// ユーザー登録
        /// </summary>
        [HttpPost("Regist")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Regist([FromBody] RegistUserRequest request)
        {
            try
            {
                // メールアドレス重複チェック
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.DeleteDate == null);

                if (existingUser != null)
                {
                    return Ok(HttpResponseService.Fail<LoginResponse>("このメールアドレスは既に登録されています"));
                }

                // ユーザー作成
                var user = new Users
                {
                    Name = request.UserName,
                    UserHash = request.UserHash,
                    Email = request.Email,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // 家計簿作成
                var kakeibo = new Kakeibo
                {
                    UserId = user.Id,
                    KakeiboName = request.KakeiboName,
                    KakeiboExplanation = request.KakeiboExplanation,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _context.Kakeibos.Add(kakeibo);
                await _context.SaveChangesAsync();

                // JWTトークンを生成
                var token = _jwtService.GenerateToken(user.Id, user.Email);

                var response = new LoginResponse
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    Email = user.Email,
                    Token = token,
                    KakeiboId = kakeibo.Id,
                    KakeiboName = kakeibo.KakeiboName
                };

                return Ok(HttpResponseService.Ok(response, "ユーザー登録に成功しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<LoginResponse>($"ユーザー登録中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// ユーザー情報更新
        /// </summary>
        [HttpPost("Update")]
        public async Task<ActionResult<ApiResponse<object>>> Update([FromBody] UpdateUserRequest request)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Kakeibos.Where(k => k.DeleteDate == null))
                    .FirstOrDefaultAsync(u => u.Id == request.UserId && u.DeleteDate == null);

                if (user == null)
                {
                    return Ok(HttpResponseService.Fail<object>("ユーザーが見つかりません"));
                }

                var kakeibo = user.Kakeibos.FirstOrDefault();
                if (kakeibo == null)
                {
                    return Ok(HttpResponseService.Fail<object>("家計簿が見つかりません"));
                }

                // ユーザー情報更新
                user.Name = request.UserName;
                user.Email = request.Email;
                user.UpdateDate = DateTime.UtcNow;

                // 家計簿情報更新
                kakeibo.KakeiboName = request.KakeiboName;
                kakeibo.KakeiboExplanation = request.KakeiboExplanation;
                kakeibo.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(HttpResponseService.Ok<object>(null, "ユーザー情報を更新しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"ユーザー情報更新中にエラーが発生しました: {ex.Message}"));
            }
        }

        /// <summary>
        /// ユーザー削除（論理削除）
        /// </summary>
        [HttpPost("Delete")]
        public async Task<ActionResult<ApiResponse<object>>> Delete([FromBody] DeleteUserRequest request)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == request.UserId && u.DeleteDate == null);

                if (user == null)
                {
                    return Ok(HttpResponseService.Fail<object>("ユーザーが見つかりません"));
                }

                // 論理削除
                user.DeleteDate = DateTime.UtcNow;
                user.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(HttpResponseService.Ok<object>(null, "ユーザーを削除しました"));
            }
            catch (Exception ex)
            {
                return Ok(HttpResponseService.Fail<object>($"ユーザー削除中にエラーが発生しました: {ex.Message}"));
            }
        }
    }
}

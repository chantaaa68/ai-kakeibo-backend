using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            try
            {
                // ユーザー検索 (メールアドレスとハッシュ値で認証)
                var user = await _userRepository.GetByEmailAndHashAsync(request.Email, request.UserHash);

                if (user == null)
                {
                    return ApiResponseHelper.Fail("メールアドレスまたはパスワードが正しくありません");
                }

                // 最初のKakeiboを取得
                var kakeibo = user.Kakeibos.FirstOrDefault();
                if (kakeibo == null)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
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

                return ApiResponseHelper.Success(response, "ログインに成功しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ログイン処理中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetUserDataAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null)
                {
                    return ApiResponseHelper.Fail("ユーザーが見つかりません");
                }

                var kakeibo = user.Kakeibos.FirstOrDefault();
                if (kakeibo == null)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
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

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ユーザー情報取得中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> RegistAsync(RegistUserRequest request)
        {
            try
            {
                // メールアドレス重複チェック
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);

                if (existingUser != null)
                {
                    return ApiResponseHelper.Fail("このメールアドレスは既に登録されています");
                }

                // ユーザー作成
                var user = new Users
                {
                    Name = request.UserName,
                    UserHash = request.UserHash,
                    Email = request.Email
                };

                await _userRepository.CreateUserAsync(user);

                // 家計簿作成
                var kakeibo = new Kakeibo
                {
                    UserId = user.Id,
                    KakeiboName = request.KakeiboName,
                    KakeiboExplanation = request.KakeiboExplanation
                };

                await _userRepository.CreateKakeiboAsync(kakeibo);

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

                return ApiResponseHelper.Success(response, "ユーザー登録に成功しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ユーザー登録中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateAsync(UpdateUserRequest request)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);

                if (user == null)
                {
                    return ApiResponseHelper.Fail("ユーザーが見つかりません");
                }

                var kakeibo = user.Kakeibos.FirstOrDefault();
                if (kakeibo == null)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
                }

                // ユーザー情報更新
                user.Name = request.UserName;
                user.Email = request.Email;
                await _userRepository.UpdateUserAsync(user);

                // 家計簿情報更新
                kakeibo.KakeiboName = request.KakeiboName;
                kakeibo.KakeiboExplanation = request.KakeiboExplanation;
                await _userRepository.UpdateKakeiboAsync(kakeibo);

                return ApiResponseHelper.Success<object>(null, "ユーザー情報を更新しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ユーザー情報更新中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteAsync(DeleteUserRequest request)
        {
            try
            {
                await _userRepository.DeleteUserAsync(request.UserId);
                return ApiResponseHelper.Success<object>(null, "ユーザーを削除しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ユーザー削除中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}

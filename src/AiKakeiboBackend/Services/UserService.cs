using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    [Service]
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        /// <summary>
        /// ユーザーのログイン処理を実行します。
        /// メールアドレスとパスワードハッシュによる認証を行い、成功時にJWTトークンとユーザーID、家計簿IDを返却します。
        /// </summary>
        /// <param name="request">ログインリクエスト（メールアドレス、ユーザーハッシュを含む）</param>
        /// <returns>ログイン成功時はユーザーID、JWTトークン、家計簿IDを含むApiResponse。失敗時はエラーメッセージを含むApiResponse</returns>
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            try
            {
                // ユーザー検索 (メールアドレスとハッシュ値で認証)
                Users? user = await _userRepository.GetByEmailAndHashAsync(request.Email, request.UserHash);

                if (user == null)
                {
                    return ApiResponseHelper.Fail("メールアドレスまたはパスワードが正しくありません");
                }

                // 最初のKakeiboを取得
                Kakeibo? kakeibo = user.Kakeibos.FirstOrDefault();
                if (kakeibo == null)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
                }

                // JWTトークンを生成
                string token = _jwtService.GenerateToken(user.Id, user.Email);

                LoginResponse response = new LoginResponse
                {
                    UserId = user.Id,
                    Token = token,
                    KakeiboId = kakeibo.Id
                };

                return ApiResponseHelper.Success(response, "ログインに成功しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ログイン処理中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 指定されたユーザーIDに基づいてユーザー情報と関連する家計簿情報を取得します。
        /// </summary>
        /// <param name="request">ユーザーデータ取得リクエスト（ユーザーIDを含む）</param>
        /// <returns>ユーザー情報と家計簿情報を含むApiResponse。ユーザーまたは家計簿が存在しない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> GetUserDataAsync(GetUserDataRequest request)
        {
            try
            {
                Users? user = await _userRepository.GetByIdAsync(request.UserId);

                if (user == null)
                {
                    return ApiResponseHelper.Fail("ユーザーが見つかりません");
                }

                Kakeibo? kakeibo = user.Kakeibos.FirstOrDefault();
                if (kakeibo == null)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
                }

                GetUserDataResponse response = new GetUserDataResponse
                {
                    UserName = user.Name,
                    Email = user.Email,
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

        /// <summary>
        /// 新規ユーザーの登録処理を実行します。
        /// ユーザー情報の作成と初期家計簿の作成を一連の流れで行います。
        /// メールアドレスの重複チェックを実施し、既存の場合はエラーを返却します。
        /// </summary>
        /// <param name="request">ユーザー登録リクエスト（ユーザー名、メール、パスワードハッシュ、家計簿名等を含む）</param>
        /// <returns>登録成功時はユーザーIDを含むApiResponse。失敗時はエラーメッセージを返却</returns>
        public async Task<IActionResult> RegistAsync(RegistUserRequest request)
        {
            try
            {
                // メールアドレス重複チェック
                Users? existingUser = await _userRepository.GetByEmailAsync(request.Email);

                if (existingUser != null)
                {
                    return ApiResponseHelper.Fail("このメールアドレスは既に登録されています");
                }

                // ユーザー作成
                Users user = new Users
                {
                    Name = request.UserName,
                    UserHash = request.UserHash,
                    Email = request.Email
                };

                await _userRepository.CreateUserAsync(user);

                // 家計簿作成
                Kakeibo kakeibo = new Kakeibo
                {
                    UserId = user.Id,
                    KakeiboName = request.KakeiboName,
                    KakeiboExplanation = request.KakeiboExplanation
                };

                await _userRepository.CreateKakeiboAsync(kakeibo);

                RegistUserResponse response = new RegistUserResponse
                {
                    UserId = user.Id
                };

                return ApiResponseHelper.Success(response, "ユーザー登録に成功しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ユーザー登録中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// ユーザー情報と関連する家計簿情報の更新処理を実行します。
        /// ユーザー名、メールアドレス、家計簿名、家計簿説明を更新します。
        /// </summary>
        /// <param name="request">ユーザー更新リクエスト（ユーザーID、更新後のユーザー名、メール、家計簿情報を含む）</param>
        /// <returns>更新成功時は成功メッセージを含むApiResponse。ユーザーまたは家計簿が存在しない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> UpdateAsync(UpdateUserRequest request)
        {
            try
            {
                Users? user = await _userRepository.GetByIdAsync(request.UserId);

                if (user == null)
                {
                    return ApiResponseHelper.Fail("ユーザーが見つかりません");
                }

                // ユーザー情報更新（nullでない場合のみ）
                if (request.UserName != null)
                {
                    user.Name = request.UserName;
                }
                if (request.Email != null)
                {
                    user.Email = request.Email;
                }
                await _userRepository.UpdateUserAsync(user);

                // 家計簿情報更新（家計簿関連のプロパティが指定されている場合）
                if (request.KakeiboName != null || request.KakeiboExplanation != null)
                {
                    Kakeibo? kakeibo = user.Kakeibos.FirstOrDefault();
                    if (kakeibo == null)
                    {
                        return ApiResponseHelper.Fail("家計簿が見つかりません");
                    }

                    if (request.KakeiboName != null)
                    {
                        kakeibo.KakeiboName = request.KakeiboName;
                    }
                    if (request.KakeiboExplanation != null)
                    {
                        kakeibo.KakeiboExplanation = request.KakeiboExplanation;
                    }
                    await _userRepository.UpdateKakeiboAsync(kakeibo);
                }

                return ApiResponseHelper.Success<object>(null, "ユーザー情報を更新しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ユーザー情報更新中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 指定されたユーザーIDのユーザーを削除します。
        /// カスケード削除により関連する家計簿データも合わせて削除されます。
        /// </summary>
        /// <param name="request">ユーザー削除リクエスト（削除対象のユーザーIDを含む）</param>
        /// <returns>削除成功時は削除されたユーザーIDを含むApiResponse。削除失敗時はエラーメッセージを返却</returns>
        public async Task<IActionResult> DeleteAsync(DeleteUserRequest request)
        {
            try
            {
                await _userRepository.DeleteUserAsync(request.UserId);

                DeleteUserResponse response = new DeleteUserResponse
                {
                    UserId = request.UserId
                };

                return ApiResponseHelper.Success(response, "ユーザーを削除しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"ユーザー削除中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}

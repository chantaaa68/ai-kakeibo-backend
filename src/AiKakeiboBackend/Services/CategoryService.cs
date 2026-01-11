using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    [Service]
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// カテゴリ情報を取得します。
        /// ユーザーIDが指定された場合はユーザー固有のカテゴリを、デフォルトフラグがtrueの場合はシステムデフォルトカテゴリを取得します。
        /// 両方の条件を指定することで、ユーザーカテゴリとデフォルトカテゴリの両方を取得可能です。
        /// </summary>
        /// <param name="request">カテゴリデータ取得リクエスト（ユーザーID、デフォルトフラグを含む）</param>
        /// <returns>カテゴリリストを含むApiResponse。取得成功時はカテゴリ情報の配列、失敗時はエラーメッセージを返却</returns>
        public async Task<IActionResult> GetCategoryDataAsync(GetCategoryDataRequest request)
        {
            try
            {
                var categories = new List<CategoryItem>();

                // ユーザー固有のカテゴリを取得（DefaultFlgがfalseの場合）
                if (!request.DefaultFlg)
                {
                    var kakeibo = await _userRepository.GetKakeiboByUserIdAsync(request.UserId);

                    if (kakeibo != null)
                    {
                        var userCategories = await _categoryRepository.GetCategoriesByKakeiboIdAsync(kakeibo.Id);
                        categories.AddRange(userCategories.Select(c => new CategoryItem
                        {
                            Id = c.Id,
                            CategoryName = c.CategoryName,
                            InoutFlg = c.InoutFlg,
                            IconName = c.Icon?.OfficialIconName ?? string.Empty
                        }));
                    }
                }

                // デフォルトカテゴリを取得（DefaultFlgがtrueの場合）
                if (request.DefaultFlg)
                {
                    var defaultCategories = await _categoryRepository.GetDefaultCategoriesAsync();
                    categories.AddRange(defaultCategories.Select(c => new CategoryItem
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        InoutFlg = c.InoutFlg,
                        IconName = c.Icon?.OfficialIconName ?? string.Empty
                    }));
                }

                var response = new GetCategoryDataResponse
                {
                    Categories = categories
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"カテゴリデータ取得中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 新規カテゴリの登録処理を実行します。
        /// ユーザーIDから家計簿IDを特定し、指定されたアイコン名に対応するアイコンIDを取得して、カテゴリを作成します。
        /// </summary>
        /// <param name="request">カテゴリ登録リクエスト（ユーザーID、カテゴリ名、入出金フラグ、アイコン名を含む）</param>
        /// <returns>登録成功時はカテゴリIDを含むApiResponse。家計簿またはアイコンが見つからない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> RegistCategoryAsync(RegistCategoryRequest request)
        {
            try
            {
                // UserIdからKakeiboIdを取得
                var kakeibo = await _userRepository.GetKakeiboByUserIdAsync(request.UserId);

                if (kakeibo == null)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
                }

                // IconNameからIconを取得
                var icon = await _categoryRepository.GetIconByNameAsync(request.IconName);

                if (icon == null)
                {
                    return ApiResponseHelper.Fail("指定されたアイコンが見つかりません");
                }

                // カテゴリ作成
                var category = new Category
                {
                    KakeiboID = kakeibo.Id,
                    CategoryName = request.CategoryName,
                    InoutFlg = request.InoutFlg,
                    IconId = icon.Id
                };

                await _categoryRepository.CreateCategoryAsync(category);

                var response = new RegistCategoryResponse
                {
                    CategoryId = category.Id
                };

                return ApiResponseHelper.Success(response, "カテゴリを登録しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"カテゴリ登録中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 既存カテゴリの情報を更新します。
        /// カテゴリ名、入出金フラグ、アイコンを個別または複数同時に更新可能です。
        /// 各パラメータは省略可能で、指定された項目のみ更新されます。
        /// </summary>
        /// <param name="request">カテゴリ更新リクエスト（カテゴリID、更新後のカテゴリ名、入出金フラグ、アイコン名を含む）</param>
        /// <returns>更新成功時はカテゴリIDを含むApiResponse。カテゴリまたはアイコンが見つからない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(request.Id);

                if (category == null)
                {
                    return ApiResponseHelper.Fail("カテゴリが見つかりません");
                }

                // カテゴリ名の更新
                if (!string.IsNullOrEmpty(request.CategoryName))
                {
                    category.CategoryName = request.CategoryName;
                }

                // 出入金フラグの更新
                if (request.InoutFlg.HasValue)
                {
                    category.InoutFlg = request.InoutFlg.Value;
                }

                // アイコンの更新
                if (!string.IsNullOrEmpty(request.IconName))
                {
                    var icon = await _categoryRepository.GetIconByNameAsync(request.IconName);

                    if (icon == null)
                    {
                        return ApiResponseHelper.Fail("指定されたアイコンが見つかりません");
                    }

                    category.IconId = icon.Id;
                }

                await _categoryRepository.UpdateCategoryAsync(category);

                var response = new UpdateCategoryResponse
                {
                    CategoryId = category.Id
                };

                return ApiResponseHelper.Success(response, "カテゴリを更新しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"カテゴリ更新中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}

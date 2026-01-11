using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> GetCategoryDataAsync(int? userId, bool? defaultFlg)
        {
            try
            {
                var categories = new List<CategoryDto>();

                // ユーザー固有のカテゴリを取得
                if (userId.HasValue)
                {
                    var kakeibo = await _userRepository.GetKakeiboByUserIdAsync(userId.Value);

                    if (kakeibo != null)
                    {
                        var userCategories = await _categoryRepository.GetCategoriesByKakeiboIdAsync(kakeibo.Id);
                        categories.AddRange(userCategories);
                    }
                }

                // デフォルトカテゴリを取得
                if (defaultFlg == true)
                {
                    var defaultCategories = await _categoryRepository.GetDefaultCategoriesAsync();
                    categories.AddRange(defaultCategories);
                }

                var response = new CategoryListResponse
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

                return ApiResponseHelper.Success<object>(null, "カテゴリを登録しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"カテゴリ登録中にエラーが発生しました: {ex.Message}");
            }
        }

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

                return ApiResponseHelper.Success<object>(null, "カテゴリを更新しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"カテゴリ更新中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}

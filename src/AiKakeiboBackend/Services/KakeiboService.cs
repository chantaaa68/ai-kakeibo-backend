using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public class KakeiboService : IKakeiboService
    {
        private readonly IKakeiboRepository _kakeiboRepository;
        private readonly IUserRepository _userRepository;

        public KakeiboService(IKakeiboRepository kakeiboRepository, IUserRepository userRepository)
        {
            _kakeiboRepository = kakeiboRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> UpdateKakeiboAsync(UpdateKakeiboRequest request)
        {
            try
            {
                var kakeibo = await _kakeiboRepository.GetByIdAsync(request.Id);

                if (kakeibo == null)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
                }

                kakeibo.KakeiboName = request.KakeiboName;
                kakeibo.KakeiboExplanation = request.KakeiboExplanation;
                await _userRepository.UpdateKakeiboAsync(kakeibo);

                return ApiResponseHelper.Success<object>(null, "家計簿情報を更新しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"家計簿情報更新中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetMonthlyResultAsync(int userId)
        {
            try
            {
                // ユーザーIDから家計簿IDを取得
                int? kakeiboId = await _kakeiboRepository.GetKakeiboIdAsync(userId);

                if (kakeiboId == null || kakeiboId == 0)
                {
                    return ApiResponseHelper.Fail("家計簿が存在しません");
                }

                var now = DateTime.UtcNow;
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1);

                // 当月のアイテムを取得
                var items = await _kakeiboRepository.GetItemsByKakeiboIdForMonthAsync(kakeiboId.Value, startOfMonth, endOfMonth);

                // 収入・支出の合計計算
                var totalIncome = items.Where(i => i.InoutFlg == true).Sum(i => i.ItemAmount);
                var totalExpense = items.Where(i => i.InoutFlg == false).Sum(i => i.ItemAmount);

                // カテゴリ別集計
                var categorySummaries = items
                    .GroupBy(i => new { i.CategoryId, i.Category.CategoryName, i.InoutFlg })
                    .Select(g => new CategorySummaryDto
                    {
                        CategoryId = g.Key.CategoryId,
                        CategoryName = g.Key.CategoryName,
                        InoutFlg = g.Key.InoutFlg,
                        TotalAmount = g.Sum(i => i.ItemAmount)
                    })
                    .ToList();

                var response = new MonthlyResultDto
                {
                    Year = now.Year,
                    Month = now.Month,
                    TotalIncome = totalIncome,
                    TotalExpense = totalExpense,
                    Balance = totalIncome - totalExpense,
                    CategorySummaries = categorySummaries
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"月次結果取得中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetKakeiboItemListAsync(int userId, string range)
        {
            try
            {
                // UserIdからKakeiboIdを取得
                int? kakeiboId = await _kakeiboRepository.GetKakeiboIdAsync(userId);

                if (kakeiboId == null || kakeiboId == 0)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
                }

                // Range（期間）のパース
                DateTime? startDate = null;
                DateTime? endDate = null;

                if (!string.IsNullOrEmpty(range))
                {
                    var parts = range.Split('-');
                    if (parts.Length >= 2)
                    {
                        int year = int.Parse(parts[0]);
                        int month = int.Parse(parts[1]);
                        startDate = new DateTime(year, month, 1);
                        endDate = startDate.Value.AddMonths(1);
                    }
                }

                var items = await _kakeiboRepository.GetItemsByKakeiboIdAndRangeAsync(kakeiboId.Value, startDate, endDate);

                var itemDtos = items.Select(i => new KakeiboItemDto
                {
                    Id = i.Id,
                    ItemName = i.ItemName ?? string.Empty,
                    ItemAmount = i.ItemAmount,
                    InoutFlg = i.InoutFlg,
                    UsedDate = i.UsedDate,
                    CategoryId = i.CategoryId,
                    CategoryName = i.Category.CategoryName,
                    Frequency = i.KakeiboItemFrequency.Frequency
                }).ToList();

                var response = new KakeiboItemListResponse
                {
                    Items = itemDtos
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイテムリスト取得中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetKakeiboItemDetailAsync(int itemId)
        {
            try
            {
                var item = await _kakeiboRepository.GetItemByIdAsync(itemId);

                if (item == null)
                {
                    return ApiResponseHelper.Fail("アイテムが見つかりません");
                }

                var response = new KakeiboItemDetailDto
                {
                    Id = item.Id,
                    ItemName = item.ItemName ?? string.Empty,
                    ItemAmount = item.ItemAmount,
                    InoutFlg = item.InoutFlg,
                    UsedDate = item.UsedDate,
                    CategoryId = item.CategoryId,
                    CategoryName = item.Category.CategoryName,
                    Frequency = item.KakeiboItemFrequency.Frequency,
                    FixedEndDate = item.KakeiboItemFrequency.FixedEndDate,
                    CreateDate = item.CreateDate,
                    UpdateDate = item.UpdateDate
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイテム詳細取得中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> RegistKakeiboItemAsync(RegistKakeiboItemRequest request)
        {
            try
            {
                // KakeiboIdの検証
                var kakeibo = await _kakeiboRepository.GetByIdAsync(request.KakeiboId);

                if (kakeibo == null)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
                }

                // カテゴリの検証
                var category = await _kakeiboRepository.GetCategoryByIdAsync(request.CategoryId);

                if (category == null)
                {
                    return ApiResponseHelper.Fail("カテゴリが見つかりません");
                }

                // KakeiboItemFrequency作成
                var frequency = new KakeiboItemFrequency
                {
                    KakeiboId = request.KakeiboId,
                    CategoryId = request.CategoryId,
                    ItemName = request.ItemName,
                    ItemAmount = request.ItemAmount,
                    InoutFlg = request.InoutFlg,
                    Frequency = request.Frequency,
                    FixedStartDate = request.UsedDate,
                    FixedEndDate = request.FixedEndDate
                };

                await _kakeiboRepository.CreateFrequencyAsync(frequency);

                // KakeiboItem作成
                var item = new KakeiboItem
                {
                    KakeiboId = request.KakeiboId,
                    CategoryId = request.CategoryId,
                    ItemName = request.ItemName,
                    ItemAmount = request.ItemAmount,
                    InoutFlg = request.InoutFlg,
                    UsedDate = request.UsedDate,
                    FrequencyId = frequency.Id
                };

                await _kakeiboRepository.CreateItemAsync(item);

                return ApiResponseHelper.Success<object>(null, "アイテムを登録しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイテム登録中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateKakeiboItemAsync(UpdateKakeiboItemRequest request)
        {
            try
            {
                var item = await _kakeiboRepository.GetItemByIdAsync(request.ItemId);

                if (item == null)
                {
                    return ApiResponseHelper.Fail("アイテムが見つかりません");
                }

                // カテゴリの検証
                var category = await _kakeiboRepository.GetCategoryByIdAsync(request.CategoryId);

                if (category == null)
                {
                    return ApiResponseHelper.Fail("カテゴリが見つかりません");
                }

                // アイテム情報更新
                item.CategoryId = request.CategoryId;
                item.ItemName = request.ItemName;
                item.ItemAmount = request.ItemAmount;
                item.InoutFlg = request.InoutFlg;
                item.UsedDate = request.UsedDate;

                // changeFlagがtrueの場合、Frequency情報も更新
                if (request.ChangeFlg && item.KakeiboItemFrequency != null)
                {
                    item.KakeiboItemFrequency.CategoryId = request.CategoryId;
                    item.KakeiboItemFrequency.ItemName = request.ItemName;
                    item.KakeiboItemFrequency.ItemAmount = request.ItemAmount;
                    item.KakeiboItemFrequency.InoutFlg = request.InoutFlg;
                }

                await _kakeiboRepository.UpdateItemAsync(item);

                return ApiResponseHelper.Success<object>(null, "アイテムを更新しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイテム更新中にエラーが発生しました: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteKakeiboItemAsync(DeleteKakeiboItemRequest request)
        {
            try
            {
                await _kakeiboRepository.DeleteItemAsync(request.Id);
                return ApiResponseHelper.Success<object>(null, "アイテムを削除しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイテム削除中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}

using AiKakeiboBackend.Attributes;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Helpers;
using AiKakeiboBackend.Models;
using AiKakeiboBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    [Service]
    public class KakeiboService : IKakeiboService
    {
        private readonly IKakeiboRepository _kakeiboRepository;
        private readonly IUserRepository _userRepository;

        public KakeiboService(IKakeiboRepository kakeiboRepository, IUserRepository userRepository)
        {
            _kakeiboRepository = kakeiboRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 家計簿の基本情報を更新します。
        /// 家計簿名と家計簿説明を更新対象とします。
        /// </summary>
        /// <param name="request">家計簿更新リクエスト（家計簿ID、更新後の家計簿名、家計簿説明を含む）</param>
        /// <returns>更新成功時は更新件数を含むApiResponse。家計簿が見つからない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> UpdateKakeiboAsync(UpdateKakeiboRequest request)
        {
            try
            {
                Kakeibo? kakeibo = await _kakeiboRepository.GetByIdAsync(request.Id);

                if (kakeibo == null)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
                }

                // nullでない場合のみ更新
                if (request.KakeiboName != null)
                {
                    kakeibo.KakeiboName = request.KakeiboName;
                }
                if (request.KakeiboExplanation != null)
                {
                    kakeibo.KakeiboExplanation = request.KakeiboExplanation;
                }

                await _userRepository.UpdateKakeiboAsync(kakeibo);

                UpdateKakeiboResponse response = new UpdateKakeiboResponse
                {
                    Count = 1
                };

                return ApiResponseHelper.Success(response, "家計簿情報を更新しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"家計簿情報更新中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 全期間の月次集計結果を取得します。
        /// 収入・支出別に、年月ごとのカテゴリ別集計結果を算出します。
        /// パフォーマンス最適化：必要なカラムのみを射影し、AsNoTracking()でトラッキングを無効化
        /// </summary>
        /// <param name="request">全期間月次集計結果取得リクエスト（ユーザーIDを含む）</param>
        /// <returns>全期間月次集計結果を含むApiResponse。家計簿が存在しない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> GetMonthlyReportAsync(GetMonthlyReportRequest request)
        {
            try
            {
                // ユーザーIDから家計簿IDを取得
                int? kakeiboId = await _kakeiboRepository.GetKakeiboIdAsync(request.UserId);

                if (kakeiboId == null || kakeiboId == 0)
                {
                    return ApiResponseHelper.Fail("家計簿が存在しません");
                }

                // 最適化版：必要なデータのみを射影し、メモリ上でグルーピング
                List<MonthlyReport> monthlyReports = await _kakeiboRepository.GetMonthlyReportDataAsync(kakeiboId.Value);

                GetMonthlyReportResult response = new GetMonthlyReportResult
                {
                    MonthlyReports = monthlyReports
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"月次集計取得中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 当月の家計簿集計結果を取得します。
        /// 当月の収入合計、支出合計、およびカテゴリ別の集計結果を算出します。
        /// </summary>
        /// <param name="request">月次集計結果取得リクエスト（ユーザーIDを含む）</param>
        /// <returns>月次集計結果を含むApiResponse。家計簿が存在しない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> GetMonthlyResultAsync(GetMonthlyResultRequest request)
        {
            try
            {
                // ユーザーIDから家計簿IDを取得
                int? kakeiboId = await _kakeiboRepository.GetKakeiboIdAsync(request.UserId);

                if (kakeiboId == null || kakeiboId == 0)
                {
                    return ApiResponseHelper.Fail("家計簿が存在しません");
                }

                DateTime now = DateTime.UtcNow;
                DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1);

                // 当月のアイテムを取得
                List<KakeiboItem> items = await _kakeiboRepository.GetItemsByKakeiboIdForMonthAsync(kakeiboId.Value, startOfMonth, endOfMonth);

                // 支出のカテゴリ別集計
                List<CategoryReportItem> expenseCategories = items
                    .Where(i => !i.InoutFlg)
                    .GroupBy(i => new { i.Category.CategoryName, i.Category.Icon.OfficialIconName })
                    .Select(g => new CategoryReportItem
                    {
                        CategoryName = g.Key.CategoryName,
                        IconName = g.Key.OfficialIconName ?? string.Empty,
                        TotalAmount = g.Sum(i => i.ItemAmount)
                    })
                    .ToList();

                // 収入のカテゴリ別集計
                List<CategoryReportItem> incomeCategories = items
                    .Where(i => i.InoutFlg)
                    .GroupBy(i => new { i.Category.CategoryName, i.Category.Icon.OfficialIconName })
                    .Select(g => new CategoryReportItem
                    {
                        CategoryName = g.Key.CategoryName,
                        IconName = g.Key.OfficialIconName ?? string.Empty,
                        TotalAmount = g.Sum(i => i.ItemAmount)
                    })
                    .ToList();

                GetMonthlyResultResponse response = new GetMonthlyResultResponse
                {
                    MonthlyExpenses = new List<MonthlyReportItem>
                    {
                        new MonthlyReportItem
                        {
                            UsedMonth = $"{now.Year:D4}-{now.Month:D2}",
                            CategoryReportItems = expenseCategories
                        }
                    },
                    MonthlyIncomes = new List<MonthlyReportItem>
                    {
                        new MonthlyReportItem
                        {
                            UsedMonth = $"{now.Year:D4}-{now.Month:D2}",
                            CategoryReportItems = incomeCategories
                        }
                    }
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"月次結果取得中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 指定された期間の家計簿アイテム一覧を取得します。
        /// 期間はYYYY-MM形式で指定し、該当月の全アイテムを取得します。
        /// </summary>
        /// <param name="request">家計簿アイテムリスト取得リクエスト（ユーザーID、取得期間を含む）</param>
        /// <returns>家計簿アイテムリストを含むApiResponse。家計簿が見つからない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> GetKakeiboItemListAsync(GetKakeiboItemListRequest request)
        {
            try
            {
                // UserIdからKakeiboIdを取得
                int? kakeiboId = await _kakeiboRepository.GetKakeiboIdAsync(request.UserId);

                if (kakeiboId == null || kakeiboId == 0)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
                }

                // Range（期間）のパース
                DateTime? startDate = null;
                DateTime? endDate = null;

                if (!string.IsNullOrEmpty(request.Range))
                {
                    string[] parts = request.Range.Split('-');
                    if (parts.Length >= 2)
                    {
                        int year = int.Parse(parts[0]);
                        int month = int.Parse(parts[1]);
                        startDate = new DateTime(year, month, 1);
                        endDate = startDate.Value.AddMonths(1);
                    }
                }

                List<KakeiboItem> itemList = await _kakeiboRepository.GetItemsByKakeiboIdAndRangeAsync(kakeiboId.Value, startDate, endDate);

                // 日付でグループ化
                List<KakeiboItemInfo> groupedItems = itemList
                    .GroupBy(i => i.UsedDate.Day)
                    .Select(g => new KakeiboItemInfo
                    {
                        DayNo = g.Key,
                        Items = g.Select(i => new Item
                        {
                            ItemId = i.Id,
                            ItemName = i.ItemName ?? string.Empty,
                            ItemAmount = i.ItemAmount,
                            InoutFlg = i.InoutFlg,
                            UsedDate = i.UsedDate,
                            IconName = i.Category.Icon?.OfficialIconName ?? string.Empty
                        }).ToList()
                    })
                    .OrderBy(x => x.DayNo)
                    .ToList();

                GetKakeiboItemListResponse response = new GetKakeiboItemListResponse
                {
                    KakeiboItemInfos = groupedItems
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイテムリスト取得中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 指定された家計簿アイテムの詳細情報を取得します。
        /// アイテムの基本情報（名称、金額、入出金フラグ、使用日、カテゴリ）を取得します。
        /// </summary>
        /// <param name="request">家計簿アイテム詳細取得リクエスト（アイテムIDを含む）</param>
        /// <returns>家計簿アイテムの詳細情報を含むApiResponse。アイテムが見つからない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> GetKakeiboItemDetailAsync(GetKakeiboItemDetailRequest request)
        {
            try
            {
                KakeiboItem? item = await _kakeiboRepository.GetItemByIdAsync(request.ItemId);

                if (item == null)
                {
                    return ApiResponseHelper.Fail("アイテムが見つかりません");
                }

                GetKakeiboItemDetailResponse response = new GetKakeiboItemDetailResponse
                {
                    ItemName = item.ItemName ?? string.Empty,
                    ItemAmount = item.ItemAmount,
                    InoutFlg = item.InoutFlg,
                    UsedDate = item.UsedDate,
                    CategoryId = item.CategoryId
                };

                return ApiResponseHelper.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイテム詳細取得中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 新規家計簿アイテムを登録します。
        /// アイテムの基本情報（名称、金額、カテゴリ等）と繰り返し頻度情報を同時に作成します。
        /// 繰り返し頻度情報は固定費の管理に利用されます。
        /// Frequency が 0 以外の場合、FixedEndDate まで繰り返しアイテムを自動登録します。
        /// </summary>
        /// <param name="request">家計簿アイテム登録リクエスト（家計簿ID、カテゴリID、アイテム名、金額、入出金フラグ、使用日、頻度等を含む）</param>
        /// <returns>登録成功時は登録件数を含むApiResponse。家計簿またはカテゴリが見つからない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> RegistKakeiboItemAsync(RegistKakeiboItemRequest request)
        {
            try
            {
                // KakeiboIdの検証
                Kakeibo? kakeibo = await _kakeiboRepository.GetByIdAsync(request.KakeiboId);

                if (kakeibo == null)
                {
                    return ApiResponseHelper.Fail("家計簿が見つかりません");
                }

                // カテゴリの検証
                Category? category = await _kakeiboRepository.GetCategoryByIdAsync(request.CategoryId);

                if (category == null)
                {
                    return ApiResponseHelper.Fail("カテゴリが見つかりません");
                }

                // KakeiboItemFrequency作成
                KakeiboItemFrequency frequency = new KakeiboItemFrequency
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

                // 繰り返しアイテム登録数
                int registeredCount = 0;

                // Frequency が 0（1回限り）の場合は1件のみ登録
                if (request.Frequency == 0)
                {
                    KakeiboItem newItem = new KakeiboItem
                    {
                        KakeiboId = request.KakeiboId,
                        CategoryId = request.CategoryId,
                        ItemName = request.ItemName,
                        ItemAmount = request.ItemAmount,
                        InoutFlg = request.InoutFlg,
                        UsedDate = request.UsedDate,
                        FrequencyId = frequency.Id
                    };

                    await _kakeiboRepository.CreateItemAsync(newItem);
                    registeredCount = 1;
                }
                else
                {
                    // 固定費の場合、繰り返し登録
                    DateTime currentDate = request.UsedDate;
                    DateTime endDate = request.FixedEndDate ?? request.UsedDate;

                    while (currentDate <= endDate)
                    {
                        KakeiboItem newItem = new KakeiboItem
                        {
                            KakeiboId = request.KakeiboId,
                            CategoryId = request.CategoryId,
                            ItemName = request.ItemName,
                            ItemAmount = request.ItemAmount,
                            InoutFlg = request.InoutFlg,
                            UsedDate = currentDate,
                            FrequencyId = frequency.Id
                        };

                        await _kakeiboRepository.CreateItemAsync(newItem);
                        registeredCount++;

                        // 次の日付を計算
                        currentDate = GetNextDate(currentDate, request.Frequency);
                    }
                }

                RegistKakeiboItemResponse response = new RegistKakeiboItemResponse
                {
                    Count = registeredCount
                };

                return ApiResponseHelper.Success(response, $"{registeredCount}件のアイテムを登録しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイテム登録中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 頻度に応じて次の日付を計算します
        /// </summary>
        /// <param name="currentDate">現在の日付</param>
        /// <param name="frequency">頻度 (1:毎日, 2:1週間, 3:2週間, 4:3週間, 5:1か月, 6:2か月, 7:3か月, 8:4か月, 9:5か月, 10:6か月, 11:1年)</param>
        /// <returns>次の日付</returns>
        private DateTime GetNextDate(DateTime currentDate, int frequency)
        {
            return frequency switch
            {
                1 => currentDate.AddDays(1),      // 毎日
                2 => currentDate.AddDays(7),      // 1週間
                3 => currentDate.AddDays(14),     // 2週間
                4 => currentDate.AddDays(21),     // 3週間
                5 => currentDate.AddMonths(1),    // 1か月
                6 => currentDate.AddMonths(2),    // 2か月
                7 => currentDate.AddMonths(3),    // 3か月
                8 => currentDate.AddMonths(4),    // 4か月
                9 => currentDate.AddMonths(5),    // 5か月
                10 => currentDate.AddMonths(6),   // 6か月
                11 => currentDate.AddYears(1),    // 1年
                _ => currentDate.AddDays(1)       // デフォルトは毎日
            };
        }

        /// <summary>
        /// 既存の家計簿アイテムを更新します。
        /// アイテムの基本情報（カテゴリ、名称、金額、入出金フラグ、使用日）を更新します。
        /// changeFlgがtrueの場合、紐づく繰り返し頻度情報も同時に更新されます（固定費の一括更新）。
        /// </summary>
        /// <param name="request">家計簿アイテム更新リクエスト（アイテムID、更新後の情報、一括変更フラグを含む）</param>
        /// <returns>更新成功時は更新件数を含むApiResponse。アイテムまたはカテゴリが見つからない場合はエラーメッセージを返却</returns>
        public async Task<IActionResult> UpdateKakeiboItemAsync(UpdateKakeiboItemRequest request)
        {
            try
            {
                KakeiboItem? item = await _kakeiboRepository.GetItemByIdAsync(request.ItemId);

                if (item == null)
                {
                    return ApiResponseHelper.Fail("アイテムが見つかりません");
                }

                // カテゴリの検証
                Category? category = await _kakeiboRepository.GetCategoryByIdAsync(request.CategoryId);

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

                UpdateKakeiboItemResponse response = new UpdateKakeiboItemResponse
                {
                    Count = 1
                };

                return ApiResponseHelper.Success(response, "アイテムを更新しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイテム更新中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 指定された家計簿アイテムを削除します。
        /// アイテムに紐づく繰り返し頻度情報も合わせて削除されます。
        /// </summary>
        /// <param name="request">家計簿アイテム削除リクエスト（削除対象のアイテムIDを含む）</param>
        /// <returns>削除成功時は削除件数を含むApiResponse。削除失敗時はエラーメッセージを返却</returns>
        public async Task<IActionResult> DeleteKakeiboItemAsync(DeleteKakeiboItemRequest request)
        {
            try
            {
                await _kakeiboRepository.DeleteItemAsync(request.Id);

                DeleteKakeiboItemResponse response = new DeleteKakeiboItemResponse
                {
                    Count = 1
                };

                return ApiResponseHelper.Success(response, "アイテムを削除しました");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail($"アイテム削除中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}

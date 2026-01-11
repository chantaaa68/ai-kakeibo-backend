using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KakeiboController : ControllerBase
    {
        private readonly IKakeiboService _service;

        public KakeiboController(IKakeiboService service)
        {
            _service = service;
        }

        /// <summary>
        /// 家計簿基本情報更新APIです。家計簿の基本設定（収入、固定費、変動費の予算等）を更新します。
        /// </summary>
        /// <param name="req">家計簿更新リクエスト（KakeiboId, Income, FixedCost, VariableCost等）</param>
        /// <returns>更新結果</returns>
        [HttpPost("UpdateKakeibo")]
        public async Task<IActionResult> UpdateKakeiboAsync([FromBody] UpdateKakeiboRequest req)
        {
            return await _service.UpdateKakeiboAsync(req);
        }

        /// <summary>
        /// 月次集計取得APIです。指定されたユーザーの当月の家計簿集計結果（収入、支出、残高等）を取得します。
        /// </summary>
        /// <param name="UserId">取得対象のユーザーID</param>
        /// <returns>月次集計結果（収入、固定費、変動費、残高等）</returns>
        [HttpGet("GetMonthlyResult")]
        public async Task<IActionResult> GetMonthlyResultAsync([FromQuery] int UserId)
        {
            return await _service.GetMonthlyResultAsync(UserId);
        }

        /// <summary>
        /// 家計簿項目一覧取得APIです。指定された期間範囲の家計簿項目（収入・支出の明細）を取得します。
        /// Rangeパラメータで取得期間を指定できます（例: "2026-01"で2026年1月のデータ）。
        /// </summary>
        /// <param name="UserId">取得対象のユーザーID</param>
        /// <param name="Range">取得期間範囲（YYYY-MM形式）</param>
        /// <returns>家計簿項目のリスト（ItemId, CategoryName, Amount, Date等）</returns>
        [HttpGet("GetKakeiboItemList")]
        public async Task<IActionResult> GetKakeiboItemListAsync([FromQuery] int UserId, [FromQuery] string Range)
        {
            return await _service.GetKakeiboItemListAsync(UserId, Range);
        }

        /// <summary>
        /// 家計簿項目詳細取得APIです。指定された項目IDの詳細情報を取得します。
        /// </summary>
        /// <param name="ItemId">取得対象の項目ID</param>
        /// <returns>家計簿項目詳細（ItemId, CategoryId, Amount, Date, Memo等）</returns>
        [HttpGet("GetKakeiboItemDetail")]
        public async Task<IActionResult> GetKakeiboItemDetailAsync([FromQuery] int ItemId)
        {
            return await _service.GetKakeiboItemDetailAsync(ItemId);
        }

        /// <summary>
        /// 家計簿項目登録APIです。新規の収入・支出項目を登録します。
        /// </summary>
        /// <param name="req">家計簿項目登録リクエスト（KakeiboId, CategoryId, Amount, Date, Memo等）</param>
        /// <returns>登録結果（登録された項目情報）</returns>
        [HttpPost("RegistKakeiboItem")]
        public async Task<IActionResult> RegistKakeiboItemAsync([FromBody] RegistKakeiboItemRequest req)
        {
            return await _service.RegistKakeiboItemAsync(req);
        }

        /// <summary>
        /// 家計簿項目更新APIです。指定された項目IDの家計簿項目（金額、カテゴリ、日付、メモ等）を更新します。
        /// </summary>
        /// <param name="req">家計簿項目更新リクエスト（ItemId, CategoryId, Amount, Date, Memo等）</param>
        /// <returns>更新結果</returns>
        [HttpPost("UpdateKakeiboItem")]
        public async Task<IActionResult> UpdateKakeiboItemAsync([FromBody] UpdateKakeiboItemRequest req)
        {
            return await _service.UpdateKakeiboItemAsync(req);
        }

        /// <summary>
        /// 家計簿項目削除APIです。指定された項目IDの家計簿項目を論理削除（DeleteFlgを立てる）します。
        /// </summary>
        /// <param name="req">家計簿項目削除リクエスト（ItemId）</param>
        /// <returns>削除結果</returns>
        [HttpPost("DeleteKakeiboItem")]
        public async Task<IActionResult> DeleteKakeiboItemAsync([FromBody] DeleteKakeiboItemRequest req)
        {
            return await _service.DeleteKakeiboItemAsync(req);
        }
    }
}

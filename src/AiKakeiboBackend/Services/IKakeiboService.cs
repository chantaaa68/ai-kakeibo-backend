using AiKakeiboBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public interface IKakeiboService
    {
        Task<IActionResult> UpdateKakeiboAsync(UpdateKakeiboRequest request);
        Task<IActionResult> GetMonthlyResultAsync(GetMonthlyResultRequest request);
        Task<IActionResult> GetKakeiboItemListAsync(GetKakeiboItemListRequest request);
        Task<IActionResult> GetKakeiboItemDetailAsync(GetKakeiboItemDetailRequest request);
        Task<IActionResult> RegistKakeiboItemAsync(RegistKakeiboItemRequest request);
        Task<IActionResult> UpdateKakeiboItemAsync(UpdateKakeiboItemRequest request);
        Task<IActionResult> DeleteKakeiboItemAsync(DeleteKakeiboItemRequest request);
    }
}

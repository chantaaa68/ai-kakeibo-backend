using AiKakeiboBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public interface IKakeiboService
    {
        Task<IActionResult> UpdateKakeiboAsync(UpdateKakeiboRequest request);
        Task<IActionResult> GetMonthlyResultAsync(int userId);
        Task<IActionResult> GetKakeiboItemListAsync(int userId, string range);
        Task<IActionResult> GetKakeiboItemDetailAsync(int itemId);
        Task<IActionResult> RegistKakeiboItemAsync(RegistKakeiboItemRequest request);
        Task<IActionResult> UpdateKakeiboItemAsync(UpdateKakeiboItemRequest request);
        Task<IActionResult> DeleteKakeiboItemAsync(DeleteKakeiboItemRequest request);
    }
}

using Microsoft.AspNetCore.Mvc;

namespace AiKakeiboBackend.Services
{
    public interface IIconService
    {
        Task<IActionResult> GetIconListAsync();
    }
}
